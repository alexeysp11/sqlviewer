using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferHistoryManager
{
    Task<TransferHistoryResponseDto> GetHistoryAsync(
        Guid userUid,
        string? cursorToken,
        int limit);
}
