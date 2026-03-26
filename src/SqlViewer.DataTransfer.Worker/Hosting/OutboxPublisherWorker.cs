using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Hosting;

public sealed class OutboxPublisherWorker(
    ILogger<OutboxPublisherWorker> logger,
    IServiceScopeFactory scopeFactory,
    IKafkaProducer producer) : BackgroundService
{
    private const int DelayMs = 2000;
    private const int MessageBatchSize = 10;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in OutboxPublisherWorker");
            }
            await Task.Delay(DelayMs, stoppingToken);
        }
    }

    private async Task PublishOutboxMessagesAsync(CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        List<OutboxMessageEntity> messages = await db.OutboxMessages
            .Where(m => m.ProcessedAt == null && m.RetryCount < 5)
            .OrderBy(m => m.CreatedAt)
            .Take(MessageBatchSize)
            .ToListAsync(ct);

        foreach (OutboxMessageEntity? message in messages)
        {
            try
            {
                await producer.ProduceAsync(message.Topic, message.CorrelationId.ToString(), message.Payload);

                message.ProcessedAt = DateTime.UtcNow;
                logger.LogInformation("Published outbox message {Id} to {Topic}", message.Id, message.Topic);
            }
            catch (Exception ex)
            {
                message.RetryCount++;
                message.Error = ex.Message;
                logger.LogError(ex, "Failed to publish outbox message {Id}", message.Id);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
