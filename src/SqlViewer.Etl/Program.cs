using MassTransit;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.Common.Messages.Etl.Commands;
using SqlViewer.Etl.Services;

namespace SqlViewer.Etl;

public sealed class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();

        // MassTransit for Kafka.
        builder.Services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            x.AddRider(rider =>
            {
                rider.AddProducer<StartDataTransferCommand>("data-transfer-commands"); // Topic for commands.
                rider.UsingKafka((context, k) =>
                {
                    k.Host("localhost:9092");
                });
            });
        });

        // OpenTelemetry.
        string serviceName = "api-gateway";
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation() // Automatically catches all incoming HTTP requests
                .AddOtlpExporter(opt => {
                    // Send traces to Jaeger (the service name in Docker Compose)
                    opt.Endpoint = new Uri("http://jaeger:4317");
                }))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation() // Collects standard metrics (number of requests, etc.)
                .AddPrometheusExporter());

        WebApplication app = builder.Build();

        app.UseOpenTelemetryPrometheusScrapingEndpoint(); // Creates the /metrics page

        // Configure the HTTP request pipeline.
        app.MapGrpcService<EtlGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}