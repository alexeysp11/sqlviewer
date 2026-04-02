using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.BusinessLogic.Abstractions;

public interface ITransferHistoryManager
{
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit);
    Task<BatchTransferStatusesResponseDto> GetStatusesAsync(Guid userUid, IEnumerable<Guid> correlationIds, CancellationToken ct = default);
    Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand requestDto);
}
