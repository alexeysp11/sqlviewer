using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Services.Implementations;
using SqlViewer.Common.Services;
using SqlViewer.QueryExecution.Data.DataSeeding;
using SqlViewer.QueryExecution.Mappings;
using SqlViewer.QueryExecution.Data.DbContexts;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.QueryExecution;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<SeedMapper>();
        builder.Services.AddScoped<IQueryExecutionDataSeeder, QueryExecutionDataSeeder>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();

        builder.Services.AddDataProtection();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<QueryExecutionDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.QueryExecution))
        );

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            QueryExecutionDbContext db = scope.ServiceProvider.GetRequiredService<QueryExecutionDbContext>();
            IQueryExecutionDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<IQueryExecutionDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        //app.MapGrpcService<GreeterService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}