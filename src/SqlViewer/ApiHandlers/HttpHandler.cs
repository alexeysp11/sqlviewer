using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Constants;
using SqlViewer.Common.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;
using System.Collections.Specialized;
using System.Web;

namespace SqlViewer.ApiHandlers;

public sealed class HttpHandler : IHttpHandler
{
    private readonly HttpClient _httpClient;

    public HttpHandler()
    {
        _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(App.AppSettings.HttpClientTimeoutSeconds)
        };
    }

    public async Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Query,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        SqlQueryResponseDto responseDto = JsonConvert.DeserializeObject<SqlQueryResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.GetDbProviderDocs,
        };

        // Query parameters.
        NameValueCollection queryParams = HttpUtility.ParseQueryString(string.Empty);
        queryParams["databaseType"] = $"{(int)databaseType}";
        uriBuilder.Query = queryParams.ToString();

        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        SqlViewerDocsResponseDto responseDto = JsonConvert.DeserializeObject<SqlViewerDocsResponseDto>(jsonResponse);

        return responseDto;
    }

    public void Dispose() => _httpClient?.Dispose();
}
