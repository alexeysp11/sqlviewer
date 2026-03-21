using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.Common.Constants;
using SqlViewer.Security.Data.DataSeeding;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Data.Entities;
using SqlViewer.Security.Domain.Identities;
using SqlViewer.Security.Domain.Tokens;
using SqlViewer.Security.Mappings;
using SqlViewer.Security.Services.Grpc;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.Security;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<SeedMapper>();
        builder.Services.AddScoped<SecurityServiceMapper>();
        builder.Services.AddScoped<ISecurityDataSeeder, SecurityDataSeeder>();
        builder.Services.AddScoped<IIdentityManager, IdentityManager>();
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<SecurityDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Security))
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
            ISecurityDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<ISecurityDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        app.MapGrpcService<SecurityGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}