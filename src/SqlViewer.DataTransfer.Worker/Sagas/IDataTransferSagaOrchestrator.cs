using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.DataTransfer.Worker.Sagas;

public interface IDataTransferSagaOrchestrator
{
    Task ExecuteAsync(Guid correlationId, CancellationToken ct);
    Task UpdateSagaStateWithOutboxAsync(DataTransferSagaEntity saga, TransferSagaStatus status, string? error, CancellationToken ct);
}
