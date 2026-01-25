using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Dtos.QueryBuilder;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using SqlViewer.Common.Dtos;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class QueryBuilderHttpHandler : HttpHandler, IQueryBuilderHttpHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public QueryBuilderHttpHandler()
    {
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

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto);
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
