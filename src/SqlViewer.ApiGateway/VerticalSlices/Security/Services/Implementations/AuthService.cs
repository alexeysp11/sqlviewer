using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Auth;
using SqlViewer.Shared.Enums;

namespace SqlViewer.ApiGateway.VerticalSlices.Security.Services.Implementations;

public sealed class AuthService(
    IConfiguration config,
    ITokenService tokenService) : IAuthService
{
    public async Task<bool> VilidateByPasswordAsync(string username, string? password)
    {
        return await Task.FromResult(true);
    }

    public async Task<LoginResponseDto> CreateSessionAsync(string username)
    {
        return new LoginResponseDto
        {
            AccessToken = "",
            RefreshToken = "",
            Username = "",
            Role = SqlViewerAuthRole.Admin,
            ExpiresInSeconds = Convert.ToDouble(config[ConfigurationKeys.Jwt.ExpiryLifetimeMinutes]) * 60
        };
    }

    public async Task<LoginResponseDto> RefreshSessionAsync(string refreshToken)
    {
        return await CreateSessionAsync("");
    }

    public LoginResponseDto CreateGuestSession()
    {
        string accessToken = tokenService.GenerateAccessToken(username: null!, role: SqlViewerAuthRole.Guest);
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            Role = SqlViewerAuthRole.Guest,
            ExpiresInSeconds = Convert.ToDouble(config[ConfigurationKeys.Jwt.ExpiryLifetimeMinutes]) * 60
        };
    }
}
