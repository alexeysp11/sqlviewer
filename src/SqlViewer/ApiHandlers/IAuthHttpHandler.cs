using SqlViewer.Shared.Dtos.Auth;

namespace SqlViewer.ApiHandlers;

public interface IAuthHttpHandler : IDisposable
{
    Task<LoginResponseDto> VerifyUserByPasswordAsync(LoginRequestDto requestDto);
    Task<LoginResponseDto> GuestLoginAsync();
}
