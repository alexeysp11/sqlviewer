using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos;
using SqlViewer.Common.Dtos.Docs;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class DocsHttpHandler : HttpHandler, IDocsHttpHandler
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public DocsHttpHandler() : base()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
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
        SqlViewerDocsResponseDto responseDto = JsonSerializer.Deserialize<SqlViewerDocsResponseDto>(jsonResponse);
        return responseDto;
    }
}
