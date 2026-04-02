using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Data.Projections;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.Repositories.Abstractions;

public interface ITransferHistoryRepository
{
    Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task<IReadOnlyList<TransferJobDbProjection>> GetStatusesAsync(Guid userUid, IEnumerable<Guid> correlationIds, CancellationToken ct);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand transferCommand);
}
