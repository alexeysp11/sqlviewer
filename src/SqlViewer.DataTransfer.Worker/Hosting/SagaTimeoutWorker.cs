using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Hosting;

/// <summary>
/// Monitors active sagas and marks them as TimedOut if no updates are received within the threshold.
/// </summary>
public class SagaTimeoutWorker(IServiceScopeFactory scopeFactory, ILogger<SagaTimeoutWorker> logger) : BackgroundService
{
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromMinutes(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

            DateTime deadline = DateTime.UtcNow - _timeoutThreshold;

            // Находим саги, которые застряли в промежуточных состояниях
            List<DataTransferSagaStateEntity> stalledSagas = await db.DataTransferSagaStates
                .Where(s => s.CurrentState != TransferSagaStatus.Completed.ToString() &&
                            s.CurrentState != TransferSagaStatus.Failed.ToString() &&
                            s.CurrentState != TransferSagaStatus.TimedOut.ToString() &&
                            s.CurrentState != TransferSagaStatus.Cancelled.ToString() &&
                            s.LastUpdatedAt < deadline)
                .ToListAsync(stoppingToken);

            foreach (DataTransferSagaStateEntity saga in stalledSagas)
            {
                logger.LogWarning("Saga {Id} timed out. Last update: {Time}", saga.CorrelationId, saga.LastUpdatedAt);

                saga.CurrentState = TransferSagaStatus.TimedOut.ToString();
                saga.LastUpdatedAt = DateTime.UtcNow;

                // Добавляем команду компенсации в Outbox (на всякий случай)
                db.OutboxMessages.Add(new OutboxMessageEntity
                {
                    CorrelationId = saga.CorrelationId,
                    Topic = "worker-commands",
                    MessageType = "CleanupTargetTableCommand",
                    Payload = "{}" // Empty object or ID
                });
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
