using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferHistoryManager
{
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand requestDto);
}
