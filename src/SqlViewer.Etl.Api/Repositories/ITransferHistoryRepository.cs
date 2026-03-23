using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.Repositories;

public interface ITransferHistoryRepository
{
    Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartTransferRequestDto requestDto);
}
