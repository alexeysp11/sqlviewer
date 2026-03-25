using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog.Context;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Storage.Entities;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.Etl.Worker.BackgroundWorkers;

/// <summary>
/// A background service for reliably transmitting messages from a local Outbox table to Kafka.
/// Implements the "Transactional Outbox" pattern, guaranteeing at-least-once message delivery.
/// Processes messages in batches, supports parallel sending and a retries mechanism.
/// </summary>
public sealed class OutboxPublisherWorker(
    ILogger<OutboxPublisherWorker> logger,
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    IKafkaProducer producer) : BackgroundService
{
    public const int OutboxBatchSize = 100;
    public const int MaxRetryCount = 5;
    public const int DefaultDelayMs = 1000;
    public const int DelayExceptionMs = 5000;
    public const int DelayPostgresExceptionMs = 15000;

    private readonly ActivitySource Activity = new(configuration[Services.Observability.ServiceName]!);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(OutboxPublisherWorker)} starting...");
        while (!cancellationToken.IsCancellationRequested)
        {
            using Activity? activity = Activity.StartActivity("ProcessOutboxBatch");

            int delay = DefaultDelayMs;
            try
            {
                await ProcessBatchAsync(cancellationToken);
            }
            catch (PostgresException ex)
            {
                logger.LogError("Database error: {Message}", ex.Message);
                delay = DelayPostgresExceptionMs;
            }
            catch (Exception ex)
            {
                logger.LogError("Unexpected error in {Worker}: {Message}", nameof(OutboxPublisherWorker), ex.Message);
                delay = DelayExceptionMs;
            }
            await Task.Delay(delay, cancellationToken);
        }
    }

    public async Task ProcessBatchAsync(CancellationToken cancellationToken)
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
            return;
        }

        // Run tasks and return a tuple (message, error).
        IEnumerable<Task<(OutboxMessageEntity Message, string? Error)>> tasks = messages.Select(async msg =>
        {
            try
            {
                using (LogContext.PushProperty("CorrelationId", msg.CorrelationId))
                {
                    logger.LogInformation("Processing message for the job {CorrelationId}", msg.CorrelationId);
                    await producer.ProduceAsync(msg.Topic, msg.CorrelationId.ToString(), msg.Payload);
                }
                return (Message: msg, Error: (string?)null);
            }
            catch (Exception ex)
            {
                return (Message: msg, Error: ex.Message);
            }
        });

        (OutboxMessageEntity Message, string? Error)[] results = await Task.WhenAll(tasks);

        // Update failed messages.
        foreach ((OutboxMessageEntity Message, string? Error) res in results.Where(r => r.Error != null))
        {
            res.Message.Error = res.Error;
            res.Message.RetryCount++;
        }

        // Update successful messages.
        List<(OutboxMessageEntity Message, string? Error)> successfulResults = results.Where(r => r.Error == null).ToList();
        if (successfulResults.Count != 0)
        {
            List<Guid> successfulIds = successfulResults.Select(r => r.Message.CorrelationId).ToList();
            Dictionary<Guid, TransferJobEntity> jobsDict = await db.TransferJobs
                .Where(j => successfulIds.Contains(j.CorrelationId))
                .ToDictionaryAsync(j => j.CorrelationId, cancellationToken);

            foreach ((OutboxMessageEntity Message, string? Error) res in successfulResults)
            {
                if (jobsDict.TryGetValue(res.Message.CorrelationId, out TransferJobEntity? job))
                {
                    job.CurrentStatus = TransferStatus.Queued;
                    job.Logs.Add(new TransferStatusLogEntity
                    {
                        CorrelationId = res.Message.CorrelationId,
                        Status = TransferStatus.Queued,
                        Timestamp = DateTime.UtcNow
                    });
                }
                db.OutboxMessages.Remove(res.Message);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
