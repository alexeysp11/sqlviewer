using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.DataTransfer.Worker.Services;
using SqlViewer.Etl.Core.Services;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Constants;
using StackExchange.Redis;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.DataTransfer.Worker;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Redis.
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString(ConnectionStrings.Redis)!));

        // Kafka.
        builder.Services.AddSingleton<IKafkaProducer>(sp =>
        {
            string kafkaEndpoint = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Kafka.Url)
                ?? throw new InvalidOperationException("Unable to get Kafka URL");
            return new KafkaProducer(kafkaEndpoint);
        });

        // Saga orchestrator
        builder.Services.AddSingleton<IDataTransferSagaOrchestrator, DataTransferSagaOrchestrator>();

        // Registering the steps of the saga
        builder.Services.AddTransient<AccessabilityCheckStep>();
        builder.Services.AddTransient<SchemaValidationStep>();
        builder.Services.AddTransient<DataTransferStep>();
        builder.Services.AddTransient<CompensationStep>();

        // Services.
        builder.Services.AddScoped<IInboxService, InboxService>();

        // Workers.
        builder.Services.AddHostedService<KafkaConsumerWorker>();
        builder.Services.AddHostedService<InboxProcessorWorker>();
        builder.Services.AddHostedService<OutboxPublisherWorker>();
        builder.Services.AddHostedService<SagaTimeoutWorker>();

        builder.Services.AddDbContext<DataTransferDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.DataTransfer)));

        // Serilog.
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        // OpenTelemetry.
        string serviceName = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Observability.ServiceName)
            ?? throw new InvalidOperationException("Unable to get service name for observability");
        string jaegerEndpoint = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Observability.JaegerEndpoint)
            ?? throw new InvalidOperationException("Unable to get Jaeger endpoint for observability");
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerEndpoint)))
            .WithMetrics(metrics => metrics
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerEndpoint)));

        // Integrating Serilog logs into the overall OTel flow.
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerEndpoint));
        });

        builder.Services.AddSingleton<IMetricServer>(sp =>
        {
            ushort port = ushort.Parse(builder.Configuration[ConfigurationKeys.Services.Observability.WorkerMetricsPort]!);
            return new MetricServer(port: port, hostname: "+");
        });

        IHost host = builder.Build();

        IMetricServer metricServer = host.Services.GetRequiredService<IMetricServer>();
        metricServer.Start();

        // Initialize the database.
        using (IServiceScope scope = host.Services.CreateScope())
        {
            DataTransferDbContext dbContext = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        try
        {
            host.Run();
        }
        finally
        {
            metricServer.Stop();
        }
    }
}