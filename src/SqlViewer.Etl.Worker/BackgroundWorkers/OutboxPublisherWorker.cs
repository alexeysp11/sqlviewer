using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.BackgroundWorkers;

/// <summary>
/// A background service for reliably transmitting messages from a local Outbox table to Kafka.
/// Implements the "Transactional Outbox" pattern, guaranteeing at-least-once message delivery.
/// Processes messages in batches, supports parallel sending and a retries mechanism.
/// </summary>
public sealed class OutboxPublisherWorker(IServiceScopeFactory scopeFactory, IKafkaProducer producer) : BackgroundService
{
    public const int OutboxBatchSize = 100;
    public const int MaxRetryCount = 5;
    public const int DelayEmptyMessagesMs = 1000;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

            List<OutboxMessageEntity> messages = await db.OutboxMessages
                .Where(m => m.ProcessedAt == null && m.RetryCount < MaxRetryCount)
                .OrderBy(m => m.CreatedAt)
                .Take(OutboxBatchSize)
                .ToListAsync(cancellationToken);

            if (messages.Count == 0)
            {
                await Task.Delay(DelayEmptyMessagesMs, cancellationToken);
                continue;
            }

            // Run tasks and return a tuple (message, error).
            IEnumerable<Task<(OutboxMessageEntity Message, string? Error)>> tasks = messages.Select(async msg =>
            {
                try
                {
                    await producer.ProduceAsync(msg.Topic, msg.CorrelationId.ToString(), msg.Payload);
                    return (Message: msg, Error: (string?)null);
                }
                catch (Exception ex)
                {
                    return (Message: msg, Error: ex.Message);
                }
            });

            (OutboxMessageEntity Message, string? Error)[] results = await Task.WhenAll(tasks);

            foreach ((OutboxMessageEntity Message, string? Error) res in results)
            {
                if (res.Error == null)
                {
                    // Delete successfully sent messages.
                    db.OutboxMessages.Remove(res.Message);
                }
                else
                {
                    // Specify the error for problematic messages.
                    res.Message.Error = res.Error;
                    res.Message.RetryCount++;
                }
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
