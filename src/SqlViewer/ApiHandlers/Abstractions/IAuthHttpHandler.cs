using SqlViewer.Shared.Dtos.Auth;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IAuthHttpHandler
{
    Task<LoginResponseDto> VerifyUserByPasswordAsync(LoginRequestDto requestDto, CancellationToken ct);
    Task<LoginResponseDto> GuestLoginAsync(CancellationToken ct);
}
