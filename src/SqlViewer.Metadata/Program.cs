using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Services;
using SqlViewer.Common.Services.Implementations;
using SqlViewer.Metadata.Data.DataSeeding;
using SqlViewer.Metadata.Data.DbContexts;
using SqlViewer.Metadata.Mappings;
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

        builder.Services.AddDataProtection();

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<MetadataDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.Metadata))
        );

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            MetadataDbContext db = scope.ServiceProvider.GetRequiredService<MetadataDbContext>();
            IMetadataDataSeeder dataSeeder = scope.ServiceProvider.GetRequiredService<IMetadataDataSeeder>();
            await dataSeeder.InitializeAsync();
        }

        // Configure the HTTP request pipeline.
        //app.MapGrpcService<GreeterService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}