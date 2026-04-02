using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.StorageContexts.Abstractions;

namespace SqlViewer.ApiHandlers.Implementations;

#nullable enable

public sealed class EtlHttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory)
    : HttpHandler(userContext, httpClientFactory), IEtlHttpHandler
{
    public async Task<BatchTransferStatusesResponseDto> GetBatchTransferStatusesAsync(
        BatchTransferStatusesRequestDto requestDto,
        CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Etl.DataTransfer.GetStatus);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<BatchTransferStatusesResponseDto>(request, ct);
    }

    public async Task<StartTransferResponseDto> PostStartTransferAsync(StartTransferRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Etl.DataTransfer.Start);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<StartTransferResponseDto>(request, ct);
    }

    public async Task<TransferHistoryResponseDto> GetTransferHistoryAsync(
        Guid userUid,
        Guid? cursorTransferJobId,
        int limit,
        CancellationToken ct)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        if (cursorTransferJobId.HasValue) query["correlationId"] = cursorTransferJobId.Value.ToString();
        query["limit"] = limit.ToString();

        string path = RestApiPaths.Etl.DataTransfer.GetHistory.Replace("{userUid}", userUid.ToString(), StringComparison.Ordinal);
        string url = BuildUrl(path, query.ToString());

        using HttpRequestMessage request = CreateRequest(HttpMethod.Get, url);
        return await SendRequestAsync<TransferHistoryResponseDto>(request, ct);
    }
}
