using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Converters;
using SqlViewer.Shared.Dtos;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.StorageContexts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class SqlHttpHandler : HttpHandler, ISqlHttpHandler
{
    private readonly IUserContext _userContext;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SqlHttpHandler(IUserContext userContext) : base()
    {
        _userContext = userContext;
        _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DynamicObjectConverter() }
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
                errorMessage = string.IsNullOrEmpty(jsonResponse) ? $"Status code: {response.StatusCode}" : jsonResponse;
            }
            throw new Exception(errorMessage);
        }
        SqlQueryResponseDto responseDto = JsonSerializer.Deserialize<SqlQueryResponseDto>(jsonResponse, _jsonSerializerOptions);
        return responseDto;
    }
}
