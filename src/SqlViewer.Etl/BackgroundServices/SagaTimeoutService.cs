using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Messages.Common.Entities;
using SqlViewer.Etl.Data.DbContexts;
using SqlViewer.Etl.Enums;

namespace SqlViewer.Etl.BackgroundServices;

/// <summary>
/// Monitors active sagas and marks them as TimedOut if no updates are received within the threshold.
/// </summary>
public class SagaTimeoutService(IServiceScopeFactory scopeFactory, ILogger<SagaTimeoutService> logger) : BackgroundService
{
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromMinutes(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

            DateTime deadline = DateTime.UtcNow - _timeoutThreshold;

            // Находим саги, которые застряли в промежуточных состояниях
            var stalledSagas = await db.DataTransferSagaStates
                .Where(s => s.CurrentState != SagaStatus.Completed.ToString() &&
                            s.CurrentState != SagaStatus.Faulted.ToString() &&
                            s.CurrentState != SagaStatus.TimedOut.ToString() &&
                            s.LastUpdatedAt < deadline)
                .ToListAsync(stoppingToken);

            foreach (var saga in stalledSagas)
            {
                logger.LogWarning("Saga {Id} timed out. Last update: {Time}", saga.CorrelationId, saga.LastUpdatedAt);

                saga.CurrentState = SagaStatus.TimedOut.ToString();
                saga.LastUpdatedAt = DateTime.UtcNow;

                // Добавляем команду компенсации в Outbox (на всякий случай)
                db.OutboxMessages.Add(new OutboxMessageEntity
                {
                    CorrelationId = saga.CorrelationId,
                    Topic = "worker-commands",
                    MessageType = "CleanupTargetTableCommand",
                    Payload = "{}" // Пустой объект или ID
                });
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
