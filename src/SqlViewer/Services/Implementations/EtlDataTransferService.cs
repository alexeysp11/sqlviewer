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
        return await httpHandler.GetTransferStatusAsync(correlationId)
            ?? throw new InvalidOperationException($"Unable to get status for task {correlationId}");
    }

    public async Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit)
    {
        return await httpHandler.GetTransferHistoryAsync(userUid, cursorTransferJobId, limit) ?? new();
    }

    public void Dispose() => httpHandler?.Dispose();
}
