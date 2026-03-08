using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Factories;
using SqlViewer.Common.Factories.Implementations;
using SqlViewer.Common.Repositories;
using SqlViewer.Common.Services;
using SqlViewer.Common.Services.Implementations;
using SqlViewer.Metadata.Data.DataSeeding;
using SqlViewer.Metadata.Data.DbContexts;
using SqlViewer.Metadata.Domain.MetadataRegistries;
using SqlViewer.Metadata.Mappings;
using SqlViewer.Metadata.Repositories.Implementations;
using SqlViewer.Metadata.Services.Grpc;
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
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();
        builder.Services.AddScoped<IMetadataRegistry, MetadataRegistry>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();

        builder.Services.AddDataProtection();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<MetadataDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Metadata))
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

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            MetadataDbContext db = scope.ServiceProvider.GetRequiredService<MetadataDbContext>();
            IMetadataDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<IMetadataDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<MetadataGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}