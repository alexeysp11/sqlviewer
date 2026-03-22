using SqlViewer.Etl.Core.Data.Entities;

namespace SqlViewer.Etl.Api.Repositories;

public interface ITransferHistoryRepository
{
    Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
}
