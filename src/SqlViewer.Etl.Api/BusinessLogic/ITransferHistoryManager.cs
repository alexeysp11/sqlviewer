using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferHistoryManager
{
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartTransferRequestDto requestDto);
}
