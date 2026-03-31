using System.Net.Http;
using System.Net.Http.Json;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.StorageContexts.Abstractions;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class SqlHttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory)
    : HttpHandler(userContext, httpClientFactory), ISqlHttpHandler
{
    public async Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Sql.Query);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<SqlQueryResponseDto>(request, ct);
    }
}
