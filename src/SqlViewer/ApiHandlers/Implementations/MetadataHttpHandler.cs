using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.StorageContexts;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class MetadataHttpHandler : HttpHandler, IMetadataHttpHandler
{
    private readonly IUserContext _userContext;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public MetadataHttpHandler(IUserContext userContext) : base()
    {
        _userContext = userContext;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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

        // Authorization.
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(_userContext.TokenType, _userContext.AccessToken);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");

        // Request.
        HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;
            try
            {
                ProblemDetailsResponseDto problem = JsonSerializer.Deserialize<ProblemDetailsResponseDto>(jsonResponse, _jsonSerializerOptions);
                errorMessage = problem?.Detail ?? problem?.Title ?? jsonResponse;
            }
            catch
            {
                errorMessage = jsonResponse;
            }
            throw new Exception(errorMessage);
        }
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

        // Authorization.
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, url);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue(_userContext.TokenType, _userContext.AccessToken);
        requestMessage.Content = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");

        // Request.
        HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;
            try
            {
                ProblemDetailsResponseDto problem = JsonSerializer.Deserialize<ProblemDetailsResponseDto>(jsonResponse, _jsonSerializerOptions);
                errorMessage = problem?.Detail ?? problem?.Title ?? jsonResponse;
            }
            catch
            {
                errorMessage = jsonResponse;
            }
            throw new Exception(errorMessage);
        }
        MetadataTablesResponseDto responseDto = JsonSerializer.Deserialize<MetadataTablesResponseDto>(jsonResponse);
        return responseDto;
    }
}
