using SqlViewer.Common.Messages.Etl.Events;
using SqlViewer.Etl.Data.DbContexts;
using SqlViewer.Etl.Data.Entities;

namespace SqlViewer.Etl.Sagas;

/// <summary>
/// Orchestrates state transitions based on incoming messages from the Inbox.
/// </summary>
public class DataTransferSagaOrchestrator(EtlDbContext db)
{
    public async Task HandleAsync(Guid correlationId, object message)
    {
        DataTransferSagaStateEntity? saga = await db.DataTransferSagaStates.FindAsync(correlationId);
        if (saga is null)
            return;

        switch (message)
        {
            case DataTransferStarted started:
                saga.CurrentState = "InProgress";
                saga.StartedAt = started.StartedAt;
                break;
            case DataTransferCompleted completed:
                saga.CurrentState = "Completed";
                saga.RowsProcessed = completed.TotalRows;
                break;
            case DataTransferFailed:
                saga.CurrentState = "Faulted";
                // Trigger compensation logic here (via Outbox)
                break;
        }
        saga.LastUpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }
}
