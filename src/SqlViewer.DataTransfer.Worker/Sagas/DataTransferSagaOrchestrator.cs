using System.Text.Json;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Sagas;

/// <summary>
/// Orchestrates state transitions based on incoming messages from the Inbox.
/// </summary>
public class DataTransferSagaOrchestrator(DataTransferDbContext db)
{
    public async Task HandleAsync(Guid correlationId, object message)
    {
        DataTransferSagaStateEntity? saga = await db.DataTransferSagaStates.FindAsync(correlationId);
        if (saga is null) return;

        switch (message)
        {
            case DataTransferStarted started:
                saga.CurrentState = TransferSagaStatus.Transferring.ToString();
                saga.StartedAt = started.StartedAt;
                break;
            case DataTransferCompleted completed:
                saga.CurrentState = TransferSagaStatus.Completed.ToString();
                saga.RowsProcessed = completed.TotalRows;
                break;
            case DataTransferFailed:
                saga.CurrentState = TransferSagaStatus.Failed.ToString();
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
