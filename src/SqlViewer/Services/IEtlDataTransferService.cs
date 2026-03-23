using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services;

public interface IEtlDataTransferService : IDisposable
{
    Task<Guid> StartTransferAsync(StartTransferRequestDto request);
    Task<TransferStatusResponseDto> GetStatusAsync(Guid correlationId);
    Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit);
}
