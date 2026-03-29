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

    public async Task PublishOutboxMessagesAsync(CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Get a batch of messages
        List<OutboxMessageEntity> messages = await db.OutboxMessages
            .Where(m => m.ProcessedAt == null && m.RetryCount < 5)
            .OrderBy(m => m.CreatedAt)
            .Take(MessageBatchSize)
            .ToListAsync(ct);

        if (messages.Count == 0) return;

        // Start sending all messages in parallel
        IEnumerable<Task<(long Id, bool Success, string? Error)>> tasks = messages.Select(async m =>
        {
            try
            {
                await producer.ProduceAsync(m.Topic, m.CorrelationId.ToString(), m.Payload);
                return (Id: m.Id, Success: true, Error: (string?)null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish outbox message {Id}", m.Id);
                return (Id: m.Id, Success: false, Error: ex.Message);
            }
        });

        (long Id, bool Success, string? Error)[] results = await Task.WhenAll(tasks);

        // Separate results.
        List<long> sentIds = results.Where(r => r.Success).Select(r => r.Id).ToList();
        Dictionary<long, string?> failedResults = results.Where(r => !r.Success).ToDictionary(r => r.Id, r => r.Error);

        // Delete successfully sent messages (in one quick request).
        if (sentIds.Count != 0)
        {
            IEnumerable<OutboxMessageEntity> messagesToDelete = messages.Where(m => sentIds.Contains(m.Id));
            db.OutboxMessages.RemoveRange(messagesToDelete);
            await db.SaveChangesAsync(ct);
        }

        // Updating retrials for the failed messages (via ChangeTracker)
        if (failedResults.Count != 0)
        {
            IEnumerable<OutboxMessageEntity> failedMessages = messages.Where(m => failedResults.ContainsKey(m.Id));
            foreach (OutboxMessageEntity? m in failedMessages)
            {
                m.RetryCount++;
                m.Error = failedResults[m.Id];
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
