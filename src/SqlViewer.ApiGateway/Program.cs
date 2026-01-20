using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SqlViewer.ApiGateway.Data.DataSeeding;
using SqlViewer.ApiGateway.Data.DbContexts;
using SqlViewer.ApiGateway.Factories;
using SqlViewer.ApiGateway.Factories.Implementations;
using SqlViewer.ApiGateway.Repositories;
using SqlViewer.ApiGateway.Repositories.Implementations;
using SqlViewer.ApiGateway.Services;
using SqlViewer.ApiGateway.Services.Implementations;
using SqlViewer.Common.Models;

namespace SqlViewer.ApiGateway;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddScoped<ISqlQueryService, SqlQueryService>();
        builder.Services.AddScoped<IMetadataService, MetadataService>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IQueryBuilderService, QueryBuilderService>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IApiGatewayDataSeeder, ApiGatewayDataSeeder>();
        builder.Services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        builder.Services.AddDataProtection();
        
        builder.Services.AddDbContext<ApiGatewayDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MetadataConnection"))
        );

        string issuerSigningKey = builder.Configuration.GetValue<string>("Jwt:IssuerSigningKey")
            ?? throw new InvalidOperationException("Unable to get issuer signing key from configurations");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
                };
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            ApiGatewayDbContext db = scope.ServiceProvider.GetRequiredService<ApiGatewayDbContext>();
            IApiGatewayDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<IApiGatewayDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
