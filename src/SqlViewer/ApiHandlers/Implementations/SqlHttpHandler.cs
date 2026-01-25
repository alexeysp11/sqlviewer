using SqlViewer.Common.Constants;
using SqlViewer.Common.Converters;
using SqlViewer.Common.Dtos;
using SqlViewer.Common.Dtos.SqlQueries;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class SqlHttpHandler : HttpHandler, ISqlHttpHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SqlHttpHandler() : base()
    {
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
        SqlQueryResponseDto responseDto = JsonSerializer.Deserialize<SqlQueryResponseDto>(jsonResponse, _jsonSerializerOptions);
        return responseDto;
    }
}
