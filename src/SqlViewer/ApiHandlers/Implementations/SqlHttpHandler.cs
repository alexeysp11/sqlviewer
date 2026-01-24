using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.SqlQueries;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class SqlHttpHandler : HttpHandler, ISqlHttpHandler
{
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
}
