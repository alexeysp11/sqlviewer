using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.Repositories;

public interface ITransferHistoryRepository
{
    Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand transferCommand);
}
