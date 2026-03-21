using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Factories;
using SqlViewer.Shared.Factories.Implementations;
using SqlViewer.Shared.Repositories;
using SqlViewer.Shared.Services.Implementations;
using SqlViewer.Shared.Services;
using SqlViewer.QueryExecution.Data.DataSeeding;
using SqlViewer.QueryExecution.Mappings;
using SqlViewer.QueryExecution.Data.DbContexts;
using SqlViewer.QueryExecution.Domain.SqlQuery;
using SqlViewer.QueryExecution.Repositories.Implementations;
using SqlViewer.QueryExecution.Services.Grpc;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.QueryExecution;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<SeedMapper>();
        builder.Services.AddScoped<ISqlQueryManager, SqlQueryManager>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        builder.Services.AddScoped<IQueryExecutionDataSeeder, QueryExecutionDataSeeder>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();

        builder.Services.AddDataProtection();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<QueryExecutionDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.QueryExecution))
        );

        // OpenTelemetry.
        string serviceName = builder.Configuration.GetValue<string>(ConfigurationKeys.Services.Observability.ServiceName)
            ?? throw new InvalidOperationException("Unable to get service name for observability");
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation() // Automatically catches all incoming HTTP requests
                .AddOtlpExporter(opt => {
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

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            IQueryExecutionDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<IQueryExecutionDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        app.MapGrpcService<QueryExecutionGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}