using System.Net.Http;
using SqlViewer.Shared.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;
using SqlViewer.StorageContexts.Abstractions;
using SqlViewer.Shared.Converters;

namespace SqlViewer.ApiHandlers.Abstractions;

#nullable enable

public abstract class HttpHandler
{
    protected readonly JsonSerializerOptions _jsonSerializerOptions;
    protected readonly IUserContext? _userContext;
    protected readonly HttpClient _httpClient;

    protected HttpHandler(IUserContext userContext, IHttpClientFactory httpClientFactory) : this(httpClientFactory)
    {
        _userContext = userContext;
    }

    protected HttpHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(App.HttpClientName);
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DynamicObjectConverter() }
        };
    }

    protected static string BuildUrl(string path, string? query = "")
    {
        UriBuilder builder = new()
        {
            Scheme = App.AppSettings.ServerScheme,
            Host = App.AppSettings.ServerHost,
            Port = App.AppSettings.ServerPort,
            Path = path,
            Query = query
        };
        return builder.Uri.ToString();
    }

    protected HttpRequestMessage CreateRequest(HttpMethod method, string url)
    {
        if (string.IsNullOrEmpty(_userContext?.TokenType))
        {
            throw new InvalidOperationException("Unable to create request due to empty token type");
        }

        HttpRequestMessage request = new(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue(_userContext.TokenType, _userContext.AccessToken);
        return request;
    }

    protected async Task<T> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken ct)
    {
        using HttpResponseMessage response = await _httpClient.SendAsync(request, ct);
        string content = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw HandleApiError(response, content);
        }

        return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions)
            ?? throw new InvalidOperationException("Failed to deserialize response.");
    }

    protected Exception HandleApiError(HttpResponseMessage response, string content)
    {
        try
        {
            ProblemDetailsResponseDto? problem = JsonSerializer.Deserialize<ProblemDetailsResponseDto>(content, _jsonSerializerOptions);
            string message = problem?.Detail ?? problem?.Title ?? content;
            return new Exception(message);
        }
        catch
        {
            return new Exception(string.IsNullOrEmpty(content) ? $"Status code: {response.StatusCode}" : content);
        }
    }
}
