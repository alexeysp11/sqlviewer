using SqlViewer.Common.Dtos.Etl;

namespace SqlViewer.ApiHandlers;

public interface IEtlHttpHandler : IDisposable
{
    Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto request);
    Task<TransferStatusResponseDto> GetTransferStatusAsync(Guid correlationId);
}
