using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IEtlHttpHandler
{
    Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto request, CancellationToken ct);
    Task<BatchTransferStatusesResponseDto> GetBatchTransferStatusesAsync(BatchTransferStatusesRequestDto requestDto, CancellationToken ct);
    Task<TransferHistoryResponseDto> GetTransferHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit, CancellationToken ct);
}
