using System.Net.Http;
using System.Text.Json;

namespace SqlViewer.ApiHandlers;

public abstract class HttpHandler : IDisposable
{
    protected class ProblemDetailsResponse
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int? Status { get; set; }
    }

    protected readonly HttpClient _httpClient;
    protected readonly JsonSerializerOptions _jsonSerializerOptions;

    protected HttpHandler()
    {
        _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(App.AppSettings.HttpClientTimeoutSeconds)
        };

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public void Dispose() => _httpClient?.Dispose();
}
