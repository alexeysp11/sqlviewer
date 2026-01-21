using Newtonsoft.Json;
using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Dtos.SqlQueries;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;
using VelocipedeUtils.Shared.DbOperations.Enums;
using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Auth;

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
            Path = RestApiPaths.Sql.Query,
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
        SqlViewerDocsResponseDto responseDto = JsonConvert.DeserializeObject<SqlViewerDocsResponseDto>(jsonResponse);

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
        MetadataColumnsResponseDto responseDto = JsonConvert.DeserializeObject<MetadataColumnsResponseDto>(jsonResponse);

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
        MetadataTablesResponseDto responseDto = JsonConvert.DeserializeObject<MetadataTablesResponseDto>(jsonResponse);

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
        QueryBuilderResponseDto responseDto = JsonConvert.DeserializeObject<QueryBuilderResponseDto>(jsonResponse);

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
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        LoginResponseDto responseDto = JsonConvert.DeserializeObject<LoginResponseDto>(jsonResponse);

        return responseDto;
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
        response.EnsureSuccessStatusCode();
        string jsonResponse = await response.Content.ReadAsStringAsync();
        LoginResponseDto responseDto = JsonConvert.DeserializeObject<LoginResponseDto>(jsonResponse);

        return responseDto;
    }

    public void Dispose() => _httpClient?.Dispose();
}
