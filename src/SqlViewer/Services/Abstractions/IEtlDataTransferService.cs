using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services.Abstractions;

public interface IEtlDataTransferService
{
    Task<Guid> StartTransferAsync(StartTransferRequestDto request, CancellationToken ct = default);
    Task<BatchTransferStatusesResponseDto> GetStatusesAsync(BatchTransferStatusesRequestDto requestDto, CancellationToken ct = default);
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit, CancellationToken ct = default);
}
