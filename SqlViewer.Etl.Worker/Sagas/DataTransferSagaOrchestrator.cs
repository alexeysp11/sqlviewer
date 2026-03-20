using System.Text.Json;
using SqlViewer.Common.Messages.Etl.Events;
using SqlViewer.Common.Messages.Storage.Entities;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.Etl.Worker.Sagas;

/// <summary>
/// Orchestrates state transitions based on incoming messages from the Inbox.
/// </summary>
public class DataTransferSagaOrchestrator(EtlDbContext db)
{
    public async Task HandleAsync(Guid correlationId, object message)
    {
        DataTransferSagaStateEntity? saga = await db.DataTransferSagaStates.FindAsync(correlationId);
        if (saga is null) return;

        switch (message)
        {
            case DataTransferStarted started:
                saga.CurrentState = SagaStatus.InProgress.ToString();
                saga.StartedAt = started.StartedAt;
                break;
            case DataTransferCompleted completed:
                saga.CurrentState = SagaStatus.Completed.ToString();
                saga.RowsProcessed = completed.TotalRows;
                break;
            case DataTransferFailed:
                saga.CurrentState = SagaStatus.Faulted.ToString();
                // ЛОГИКА КОМПЕНСАЦИИ:
                // Создаем команду очистки для Worker и кладем ее в Outbox
                db.OutboxMessages.Add(new OutboxMessageEntity
                {
                    CorrelationId = correlationId,
                    Topic = "worker-commands",
                    MessageType = "CleanupTargetTableCommand",
                    Payload = JsonSerializer.Serialize(new { CorrelationId = correlationId })
                });
                break;
        }
        saga.LastUpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}
