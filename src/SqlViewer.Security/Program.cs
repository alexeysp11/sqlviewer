using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            SecurityDbContext db = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
            ISecurityDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<ISecurityDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        app.MapGrpcService<SecurityGrpcService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}