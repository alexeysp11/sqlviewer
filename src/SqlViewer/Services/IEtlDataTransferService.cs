using SqlViewer.Common.Dtos.Etl;

namespace SqlViewer.Services;

public interface IEtlDataTransferService : IDisposable
{
    Task<Guid> StartTransferAsync(StartTransferRequestDto request);
    Task<TransferStatusResponseDto> GetStatusAsync(Guid correlationId);
}
