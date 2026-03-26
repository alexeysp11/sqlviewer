using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.DataTransfer.Worker.Sagas;

public interface IDataTransferSagaOrchestrator
{
    Task ExecuteAsync(Guid correlationId, CancellationToken ct);
    Task UpdateSagaStateWithOutboxAsync(Guid correlationId, TransferSagaStatus status, string? error, CancellationToken ct);
}
