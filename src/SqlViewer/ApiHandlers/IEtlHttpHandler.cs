using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.ApiHandlers;

public interface IEtlHttpHandler : IDisposable
{
    Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto request);
    Task<TransferStatusResponseDto> GetTransferStatusAsync(Guid correlationId);
    Task<TransferHistoryResponseDto> GetTransferHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit);
}
