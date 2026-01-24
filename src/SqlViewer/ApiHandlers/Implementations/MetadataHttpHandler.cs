using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Metadata;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class MetadataHttpHandler : HttpHandler, IMetadataHttpHandler
{
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
}
