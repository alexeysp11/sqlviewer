using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace SqlViewer.ApiGateway.DelegatingHandlers;

public class TokenPropagationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? token = await httpContextAccessor.HttpContext?.GetTokenAsync("access_token")!;
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
