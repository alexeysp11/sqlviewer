using System.Net.Http;

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

    protected HttpHandler()
    {
        _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(App.AppSettings.HttpClientTimeoutSeconds)
        };
    }

    public void Dispose() => _httpClient?.Dispose();
}
