using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Hosting;

/// <summary>
/// Monitors active sagas and marks them as TimedOut if no updates are received within the threshold.
/// </summary>
public sealed class SagaTimeoutWorker(IServiceScopeFactory scopeFactory, ILogger<SagaTimeoutWorker> logger) : BackgroundService
{
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromMinutes(60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

            DateTime deadline = DateTime.UtcNow - _timeoutThreshold;

            // Find sagas that are stuck in intermediate states
            List<DataTransferSagaEntity> stalledSagas = await db.DataTransferSagas
                .Where(s => s.CurrentState != TransferSagaStatus.Completed &&
                            s.CurrentState != TransferSagaStatus.Failed &&
                            s.CurrentState != TransferSagaStatus.TimedOut &&
                            s.CurrentState != TransferSagaStatus.Cancelled &&
                            s.LastUpdatedAt < deadline)
                .ToListAsync(stoppingToken);

            foreach (DataTransferSagaEntity saga in stalledSagas)
            {
                logger.LogWarning("Saga {Id} timed out. Last update: {Time}", saga.CorrelationId, saga.LastUpdatedAt);

                saga.CurrentState = TransferSagaStatus.TimedOut;
                saga.LastUpdatedAt = DateTime.UtcNow;

                // Adding a compensation command to Outbox.
                db.OutboxMessages.Add(new OutboxMessageEntity
                {
                    CorrelationId = saga.CorrelationId,
                    Topic = "worker-commands",
                    MessageType = "CleanupTargetTableCommand",
                    Payload = "{}" // Empty object or ID
                });
            }

            // TODO: find stalled transfer executions.

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
