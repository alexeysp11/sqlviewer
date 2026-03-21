using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services.Kafka;

namespace SqlViewer.Etl.Worker.BackgroundWorkers;

/// <summary>
/// Из БД в Kafka
/// </summary>
/// <param name="scopeFactory"></param>
/// <param name="producer"></param>
public class OutboxPublisherWorker(IServiceScopeFactory scopeFactory, KafkaProducer producer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

            List<OutboxMessageEntity> messages = await db.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .Take(10)
                .ToListAsync();

            foreach (OutboxMessageEntity? msg in messages)
            {
                try
                {
                    await producer.ProduceAsync(msg.Topic, msg.CorrelationId.ToString(), msg.Payload);
                    msg.ProcessedAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    msg.Error = ex.Message;
                }
            }
            await db.SaveChangesAsync();
            await Task.Delay(1000); // Polling interval
        }
    }
}
