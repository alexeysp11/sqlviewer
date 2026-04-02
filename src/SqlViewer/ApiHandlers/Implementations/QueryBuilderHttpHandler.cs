using System.Net.Http;
using System.Net.Http.Json;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.QueryBuilder;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.StorageContexts.Abstractions;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class QueryBuilderHttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory)
    : HttpHandler(userContext, httpClientFactory), IQueryBuilderHttpHandler
{
    public async Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.QueryBuilder.CreateTable);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<QueryBuilderResponseDto>(request, ct);
    }
}
