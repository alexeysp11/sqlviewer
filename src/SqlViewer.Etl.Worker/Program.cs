using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Etl.Worker.BackgroundWorkers;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.Etl.Worker;

public sealed class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Kafka.
        builder.Services.AddSingleton<IKafkaProducer>(sp =>
        {
            string kafkaEndpoint = builder.Configuration.GetValue<string>(Services.Kafka.Url)
                ?? throw new InvalidOperationException("Unable to get Kafka URL");
            return new KafkaProducer(kafkaEndpoint);
        });

        // Database.
        builder.Services.AddDbContext<EtlDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Etl)));

        // Workers.
        builder.Services.AddHostedService<OutboxPublisherWorker>();

        // Serilog.
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        // OpenTelemetry.
        string serviceName = builder.Configuration.GetValue<string>(Services.Observability.ServiceName)
            ?? throw new InvalidOperationException("Unable to get service name for observability");
        string jaegerEndpoint = builder.Configuration.GetValue<string>(Services.Observability.JaegerEndpoint)
            ?? throw new InvalidOperationException("Unable to get Jaeger endpoint for observability");
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
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

        IHost host = builder.Build();
        host.Run();
    }
}