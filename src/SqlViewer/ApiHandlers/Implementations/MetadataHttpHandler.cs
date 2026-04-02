using System.Net.Http;
using System.Net.Http.Json;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.StorageContexts.Abstractions;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class MetadataHttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory)
    : HttpHandler(userContext, httpClientFactory), IMetadataHttpHandler
{
    public async Task<MetadataColumnsResponseDto> GetColumnsAsync(MetadataRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Metadata.Columns);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<MetadataColumnsResponseDto>(request, ct);
    }

    public async Task<MetadataTablesResponseDto> GetTables(MetadataRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Metadata.Tables);

        using HttpRequestMessage request = CreateRequest(HttpMethod.Post, url);
        request.Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions);

        return await SendRequestAsync<MetadataTablesResponseDto>(request, ct);
    }
}
