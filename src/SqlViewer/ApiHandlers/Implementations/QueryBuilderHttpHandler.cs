using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Dtos;
using SqlViewer.StorageContexts;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class QueryBuilderHttpHandler : HttpHandler, IQueryBuilderHttpHandler
{
    private readonly IUserContext _userContext;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public QueryBuilderHttpHandler(IUserContext userContext)
    {
        _userContext = userContext;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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
        QueryBuilderResponseDto responseDto = JsonSerializer.Deserialize<QueryBuilderResponseDto>(jsonResponse);
        return responseDto;
    }
}
