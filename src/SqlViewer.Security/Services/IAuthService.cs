using SqlViewer.Common.Dtos.Auth;

namespace SqlViewer.Security.Services;

public interface IAuthService
{
    Task<bool> VilidateByPasswordAsync(string username, string? password);
    Task<LoginResponseDto> CreateSessionAsync(string username);
    Task<LoginResponseDto> RefreshSessionAsync(string refreshToken);
    LoginResponseDto CreateGuestSession();
}
