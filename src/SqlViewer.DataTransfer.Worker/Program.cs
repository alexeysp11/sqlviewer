using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Consumers;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Services;
using SqlViewer.Etl.Core.Services;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.DataTransfer.Worker;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddScoped<IInboxService, DataTransferInboxService>();
        builder.Services.AddHostedService<DataTransferCommandConsumer>();

        builder.Services.AddDbContext<DataTransferDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionStrings.DataTransfer)));

        IHost host = builder.Build();

        // Initialize the database.
        using (IServiceScope scope = host.Services.CreateScope())
        {
            DataTransferDbContext dbContext = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        host.Run();
    }
}