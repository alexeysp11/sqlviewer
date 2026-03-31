using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Docs;
using SqlViewer.StorageContexts.Abstractions;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class DocsHttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory)
    : HttpHandler(userContext, httpClientFactory), IDocsHttpHandler
{
    public async Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType, CancellationToken ct)
    {
        // 1. Prepare query parameters
        NameValueCollection queryParams = HttpUtility.ParseQueryString(string.Empty);
        queryParams["databaseType"] = $"{(int)databaseType}";

        // 2. Build URL using base helper
        string url = BuildUrl(RestApiPaths.Docs.DbProviders, queryParams.ToString());

        // 3. Create authorized request and send
        using HttpRequestMessage request = CreateRequest(HttpMethod.Get, url);
        return await SendRequestAsync<SqlViewerDocsResponseDto>(request, ct);
    }
}
