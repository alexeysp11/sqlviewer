using System.Net.Http;

namespace SqlViewer.ApiHandlers;

public abstract class HttpHandler : IDisposable
{
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
