using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IEtlHttpHandler
{
    Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto request, CancellationToken ct);
    Task<List<TransferStatusResponseDto>> GetBatchTransferStatusesAsync(IEnumerable<Guid> correlationIds, CancellationToken ct);
    Task<TransferHistoryResponseDto> GetTransferHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit, CancellationToken ct);
}
