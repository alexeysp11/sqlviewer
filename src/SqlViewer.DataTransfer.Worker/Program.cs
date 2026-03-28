using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.DataTransfer.Worker.Services;
using SqlViewer.Etl.Core.Services;
using StackExchange.Redis;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.DataTransfer.Worker;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString(ConnectionStrings.Redis)!));

        // Saga orchestrator
        builder.Services.AddSingleton<IDataTransferSagaOrchestrator, DataTransferSagaOrchestrator>();

        // Registering the steps of the saga
        builder.Services.AddTransient<AccessabilityCheckStep>();
        builder.Services.AddTransient<SchemaValidationStep>();
        builder.Services.AddTransient<DataTransferStep>();
        builder.Services.AddTransient<CompensationStep>();

        builder.Services.AddScoped<IInboxService, InboxService>();
        
        builder.Services.AddHostedService<KafkaConsumerWorker>();
        builder.Services.AddHostedService<InboxProcessorWorker>();

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