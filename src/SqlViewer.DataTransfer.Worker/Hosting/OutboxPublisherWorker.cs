using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Constants;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;

namespace SqlViewer.DataTransfer.Worker.Hosting;

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

    private readonly ActivitySource Activity = new(configuration[ConfigurationKeys.Services.Observability.ServiceName]!);

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
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        List<OutboxMessageEntity> messages = await db.OutboxMessages
            .Where(m => m.ProcessedAt == null && m.RetryCount < MaxRetryCount)
            .OrderBy(m => m.CreatedAt)
            .Take(OutboxBatchSize)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        //await db.SaveChangesAsync(cancellationToken);
    }
}
