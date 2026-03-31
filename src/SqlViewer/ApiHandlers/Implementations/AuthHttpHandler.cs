using System.Net.Http;
using System.Net.Http.Json;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Auth;

namespace SqlViewer.ApiHandlers.Implementations;

public sealed class AuthHttpHandler(IHttpClientFactory httpClientFactory) : HttpHandler(httpClientFactory), IAuthHttpHandler
{
    public async Task<LoginResponseDto> VerifyUserByPasswordAsync(LoginRequestDto requestDto, CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Auth.Login);
        using HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(requestDto, options: _jsonSerializerOptions)
        };
        return await SendRequestAsync<LoginResponseDto>(request, ct);
    }

    public async Task<LoginResponseDto> GuestLoginAsync(CancellationToken ct)
    {
        string url = BuildUrl(RestApiPaths.Auth.LoginAsGuest);
        using HttpRequestMessage request = new(HttpMethod.Post, url);
        return await SendRequestAsync<LoginResponseDto>(request, ct);
    }
}
