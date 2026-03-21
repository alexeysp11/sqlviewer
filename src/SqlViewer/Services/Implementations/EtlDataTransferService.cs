using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services.Implementations;

public sealed class EtlDataTransferService(IEtlHttpHandler httpHandler) : IEtlDataTransferService
{
    public async Task<Guid> StartTransferAsync(StartTransferRequestDto request)
    {
        // Sending a request to launch Saga to API Gateway
        return (await httpHandler.PostStartTransferAsync(request)
            ?? throw new InvalidOperationException("Failed to receive response from ETL Gateway")).CorrelationId;
    }

    public async Task<TransferStatusResponseDto> GetStatusAsync(Guid correlationId)
    {
        // Querying Saga state via API Gateway
        return await httpHandler.GetTransferStatusAsync(correlationId)
            ?? throw new InvalidOperationException($"Unable to get status for task {correlationId}");
    }

    public void Dispose() => httpHandler?.Dispose();
}
