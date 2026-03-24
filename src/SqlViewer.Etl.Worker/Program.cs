using Microsoft.EntityFrameworkCore;
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

        //builder.Services.AddOpenTelemetry()
        //    .WithTracing(tracing => tracing
        //        .AddSource(serviceName)
        //        .AddHttpClientInstrumentation() // Если ETL дергает внешние API
        //        .AddEntityFrameworkCoreInstrumentation() // Видеть SQL запросы в Jaeger!
        //        .AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerEndpoint)))
        //    .WithMetrics(metrics => metrics
        //        .AddRuntimeInstrumentation() // Метрики CPU, RAM, GC
        //        .AddProcessInstrumentation()
        //        .AddHttpClientInstrumentation()
        //        .AddPrometheusExporter()); // Для Grafana

        //builder.Logging.AddOpenTelemetry(options =>
        //{
        //    options.IncludeFormattedMessage = true;
        //    options.IncludeScopes = true;
        //    options.AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerEndpoint));
        //});

        IHost host = builder.Build();
        host.Run();
    }
}