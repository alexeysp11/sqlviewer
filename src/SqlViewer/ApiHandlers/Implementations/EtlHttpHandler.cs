using System.Net.Http;
using System.Net.Http.Json;
using SqlViewer.Common.Dtos.Etl;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class EtlHttpHandler(HttpClient httpClient) : IEtlHttpHandler
{
    public async Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto request)
    {
        // Sending a POST request to the ETL endpoint in API Gateway
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/etl/transfer", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<StartTransferResponseDto>()
           ?? throw new InvalidOperationException($"Failed to deserialize {nameof(StartTransferResponseDto)}");
    }

    public async Task<TransferStatusResponseDto> GetTransferStatusAsync(Guid correlationId)
    {
        // Sending a GET request to retrieve the status of a specific saga
        return await httpClient.GetFromJsonAsync<TransferStatusResponseDto>($"api/etl/status/{correlationId}")
           ?? throw new InvalidOperationException($"Failed to deserialize {nameof(TransferStatusResponseDto)}");
    }

    public void Dispose() => httpClient?.Dispose();
}
