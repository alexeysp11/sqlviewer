using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Dtos.QueryBuilder;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class QueryBuilderHttpHandler : HttpHandler, IQueryBuilderHttpHandler
{
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
}
