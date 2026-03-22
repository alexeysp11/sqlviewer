using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SqlViewer.ApiGateway.DelegatingHandlers;
using SqlViewer.ApiGateway.Dtos.FluentValidation;
using SqlViewer.ApiGateway.Mappings;
using SqlViewer.ApiGateway.Middleware;
using SqlViewer.Etl;
using SqlViewer.Metadata;
using SqlViewer.QueryBuilder;
using SqlViewer.QueryExecution;
using SqlViewer.Security;
using SqlViewer.Shared.Constants;

namespace SqlViewer.ApiGateway;

public sealed class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddScoped<LoginMapper>();
        builder.Services.AddTransient<TokenPropagationHandler>();

        builder.Services.AddGrpcClient<SecurityService.SecurityServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.Security]!);
        });
        builder.Services.AddGrpcClient<MetadataService.MetadataServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.Metadata]!);
        }).AddHttpMessageHandler<TokenPropagationHandler>();
        builder.Services.AddGrpcClient<QueryBuilderService.QueryBuilderServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.Metadata]!);
        }).AddHttpMessageHandler<TokenPropagationHandler>();
        builder.Services.AddGrpcClient<QueryExecutionService.QueryExecutionServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.QueryExecution]!);
        }).AddHttpMessageHandler<TokenPropagationHandler>();
        builder.Services.AddGrpcClient<EtlTransferService.EtlTransferServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.Etl]!);
        }).AddHttpMessageHandler<TokenPropagationHandler>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateTableRequestValidator>();
        builder.Services.AddFluentValidationAutoValidation();

        string issuerSigningKey = builder.Configuration.GetValue<string>(ConfigurationKeys.Jwt.Key)
            ?? throw new InvalidOperationException("Unable to get issuer signing key from configurations");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
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

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Creates the /metrics page
        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GrpcExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
