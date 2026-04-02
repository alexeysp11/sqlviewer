using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Etl.Worker.Hosting;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Constants;

namespace SqlViewer.Etl.Worker;

public sealed class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Kafka.
        builder.Services.AddSingleton<IKafkaProducer>(sp =>
        {
            string kafkaEndpoint = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Kafka.Url)
                ?? throw new InvalidOperationException("Unable to get Kafka URL");
            return new KafkaProducer(kafkaEndpoint);
        });

        // Database.
        builder.Services.AddDbContext<EtlDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConfigurationKeys.ConnectionStrings.Etl)));

        // Services.
        builder.Services.AddScoped<IInboxService, InboxService>();
        builder.Services.AddScoped<ITransferStatusService, TransferStatusService>();

        // Workers.
        builder.Services.AddHostedService<InboxProcessorWorker>();
        builder.Services.AddHostedService<OutboxPublisherWorker>();
        builder.Services.AddHostedService<KafkaConsumerWorker>();

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

        builder.Services.AddMetricServer(options =>
        {
            options.Port = ushort.Parse(builder.Configuration[ConfigurationKeys.Services.Observability.WorkerMetricsPort]!);
        });

        IHost host = builder.Build();
        host.Run();
    }
}