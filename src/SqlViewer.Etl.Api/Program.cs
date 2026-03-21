using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.Shared.Constants;
using SqlViewer.Etl.Api.BusinessLogic;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Api.Services.Grpc;
using SqlViewer.Etl.Core.Data.DbContexts;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.Etl.Api;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<ITransferManager, TransferManager>();
        builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
        builder.Services.AddGrpc();

        // DbContexts.
        builder.Services.AddDbContext<EtlDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Etl))
        );

        // Kafka.

        // OpenTelemetry.
        string serviceName = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Observability.ServiceName)
            ?? throw new InvalidOperationException("Unable to get service name for observability");
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation() // Automatically catches all incoming HTTP requests
                .AddOtlpExporter(opt =>
                {
                    // Send traces to Jaeger (the service name in Docker Compose)
                    string jaegerEndpoint = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Observability.JaegerEndpoint)
                        ?? throw new InvalidOperationException("Unable to get Jaeger endpoint for observability");
                    opt.Endpoint = new Uri(jaegerEndpoint);
                }))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation() // Collects standard metrics (number of requests, etc.)
                .AddPrometheusExporter());

        WebApplication app = builder.Build();

        // Creates the /metrics page
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        // Initialize the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            EtlDbContext dbContext = scope.ServiceProvider.GetRequiredService<EtlDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        // Configure the HTTP request pipeline.
        app.MapGrpcService<EtlGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}