using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services.Abstractions;

public interface IEtlDataTransferService
{
    Task<Guid> StartTransferAsync(StartTransferRequestDto request);
    Task<BatchTransferStatusesResponseDto> GetStatusesAsync(BatchTransferStatusesRequestDto requestDto, CancellationToken ct);
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit);
}
