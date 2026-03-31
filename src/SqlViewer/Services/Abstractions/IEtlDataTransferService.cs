using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services.Abstractions;

public interface IEtlDataTransferService
{
    Task<Guid> StartTransferAsync(StartTransferRequestDto request);
    Task<List<TransferStatusResponseDto>> GetStatusesAsync(IEnumerable<Guid> correlationIds, CancellationToken ct);
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit);
}
