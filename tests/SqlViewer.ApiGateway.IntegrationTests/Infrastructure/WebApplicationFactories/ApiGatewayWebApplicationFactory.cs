using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlViewer.ApiGateway.Data.DbContexts;
using SqlViewer.ApiGateway.IntegrationTests.Infrastructure.AuthenticationHandlers;
using Testcontainers.PostgreSql;

namespace SqlViewer.ApiGateway.IntegrationTests.Infrastructure.WebApplicationFactories;

public class ApiGatewayWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:latest")
        .WithDatabase("testdb")
        .WithUsername("user")
        .WithPassword("password")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Database context.
            services.RemoveAll(typeof(DbContextOptions<ApiGatewayDbContext>));
            services.AddDbContext<ApiGatewayDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            // Authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, options => { });

            // Here you can add other mocks or initialize test data.
        });
        builder.UseEnvironment("Development");
    }

    public async Task EnsureDatabaseMigratedAsync()
    {
        using IServiceScope scope = Services.CreateScope();
        ApiGatewayDbContext db = scope.ServiceProvider.GetRequiredService<ApiGatewayDbContext>();
        await db.Database.MigrateAsync();
    }
}