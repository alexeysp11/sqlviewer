using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Factories;
using SqlViewer.Common.Factories.Implementations;
using SqlViewer.Common.Repositories;
using SqlViewer.Common.Services;
using SqlViewer.Common.Services.Implementations;
using SqlViewer.Metadata.Data.DataSeeding;
using SqlViewer.Metadata.Data.DbContexts;
using SqlViewer.Metadata.Domain.MetadataRegistries;
using SqlViewer.Metadata.Domain.QueryBuilders;
using SqlViewer.Metadata.Mappings;
using SqlViewer.Metadata.Repositories.Implementations;
using SqlViewer.Metadata.Services.Grpc;
using System.Text;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.Metadata;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<SeedMapper>();
        builder.Services.AddScoped<IMetadataDataSeeder, MetadataDataSeeder>();
        builder.Services.AddScoped<ISandboxDataSeeder, SandboxDataSeeder>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();
        builder.Services.AddScoped<IMetadataRegistry, MetadataRegistry>();
        builder.Services.AddScoped<IQueryBuilderManager, QueryBuilderManager>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();

        builder.Services.AddDataProtection();

        builder.Services.AddGrpc();

        // DbContexts.
        builder.Services.AddDbContext<MetadataDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Metadata))
        );
        builder.Services.AddDbContext<SandboxDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Sandbox))
        );

        string issuerSigningKey = builder.Configuration.GetValue<string>(ConfigurationKeys.Jwt.Key)
            ?? throw new InvalidOperationException("Unable to get issuer signing key from configurations");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration[ConfigurationKeys.Jwt.Issuer],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration[ConfigurationKeys.Jwt.Audience],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
                };
            });
        builder.Services.AddAuthorization();

        // OpenTelemetry.
        string serviceName = "metadata";
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

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            // Metadata.
            IMetadataDataSeeder metadataSeeder = scope.ServiceProvider.GetRequiredService<IMetadataDataSeeder>();
            await metadataSeeder.InitializeAsync();

            // Sandbox.
            ISandboxDataSeeder sandboxSeeder = scope.ServiceProvider.GetRequiredService<ISandboxDataSeeder>();
            await sandboxSeeder.SeedAsync();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<MetadataGrpcService>();
        app.MapGrpcService<QueryBuilderGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}