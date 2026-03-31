using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Services.Implementations;

#nullable enable

public sealed class EtlDataTransferService(IEtlHttpHandler httpHandler) : IEtlDataTransferService
{
    public async Task<Guid> StartTransferAsync(StartTransferRequestDto request)
    {
        // Sending a request to launch Saga to API Gateway
        return (await httpHandler.PostStartTransferAsync(request, default)
            ?? throw new InvalidOperationException("Failed to receive response from ETL Gateway")).CorrelationId;
    }

    public async Task<BatchTransferStatusesResponseDto> GetStatusesAsync(BatchTransferStatusesRequestDto requestDto, CancellationToken ct)
    {
        // If the list is empty, don't even call the network
        if (requestDto.CorrelationIds.Count == 0)
            return new();

        return await httpHandler.GetBatchTransferStatusesAsync(requestDto, ct) ?? new();
    }

    public async Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? cursorTransferJobId, int limit)
    {
        return await httpHandler.GetTransferHistoryAsync(userUid, cursorTransferJobId, limit, default) ?? new();
    }
}
