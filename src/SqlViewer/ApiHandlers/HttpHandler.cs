using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Dtos.QueryBuilder;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers;

public sealed class HttpHandler : IHttpHandler
{
    private class ProblemDetailsResponse
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int? Status { get; set; }
    }

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public HttpHandler()
    {
        _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(App.AppSettings.HttpClientTimeoutSeconds)
        };

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Sql.Query,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        SqlQueryResponseDto responseDto = JsonSerializer.Deserialize<SqlQueryResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Docs.DbProviders,
        };

        // Query parameters.
        NameValueCollection queryParams = HttpUtility.ParseQueryString(string.Empty);
        queryParams["databaseType"] = $"{(int)databaseType}";
        uriBuilder.Query = queryParams.ToString();

        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        SqlViewerDocsResponseDto responseDto = JsonSerializer.Deserialize<SqlViewerDocsResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<MetadataColumnsResponseDto> GetColumnsAsync(MetadataRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Metadata.Columns,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        MetadataColumnsResponseDto responseDto = JsonSerializer.Deserialize<MetadataColumnsResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<MetadataTablesResponseDto> GetTables(MetadataRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Metadata.Tables,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        MetadataTablesResponseDto responseDto = JsonSerializer.Deserialize<MetadataTablesResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.QueryBuilder.CreateTable,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        QueryBuilderResponseDto responseDto = JsonSerializer.Deserialize<QueryBuilderResponseDto>(jsonResponse);

        return responseDto;
    }

    public async Task<LoginResponseDto> VerifyUserByPasswordAsync(LoginRequestDto requestDto)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Auth.Login,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;
            try
            {
                ProblemDetailsResponse problem = JsonSerializer.Deserialize<ProblemDetailsResponse>(jsonResponse, _jsonSerializerOptions);
                errorMessage = problem?.Detail ?? problem?.Title ?? jsonResponse;
            }
            catch
            {
                errorMessage = jsonResponse;
            }

            throw new Exception(errorMessage);
        }
        return JsonSerializer.Deserialize<LoginResponseDto>(jsonResponse);
    }

    public async Task<LoginResponseDto> GuestLoginAsync()
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = RestApiPaths.Auth.LoginAsGuest,
        };
        string url = uriBuilder.Uri.ToString();

        HttpResponseMessage response = await _httpClient.PostAsync(url, null);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;
            try
            {
                ProblemDetailsResponse problem = JsonSerializer.Deserialize<ProblemDetailsResponse>(jsonResponse, _jsonSerializerOptions);
                errorMessage = problem?.Detail ?? problem?.Title ?? jsonResponse;
            }
            catch
            {
                errorMessage = jsonResponse;
            }

            throw new Exception(errorMessage);
        }
        return JsonSerializer.Deserialize<LoginResponseDto>(jsonResponse);
    }

    public void Dispose() => _httpClient?.Dispose();
}
