using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.Auth;
using SqlViewer.Shared.Enums;
using SqlViewer.StorageContexts;

namespace SqlViewer.Services.Implementations;

public sealed class AuthApiService(IAuthHttpHandler httpHandler, IUserContext userContext) : IAuthApiService
{
    public async Task<bool> VerifyUserByPasswordAsync(string username, string password)
    {
        LoginRequestDto requestDto = new()
        {
            AuthType = SqlViewerAuthType.ByPassword,
            Username = username,
            Password = password
        };

        LoginResponseDto responseDto = await httpHandler.VerifyUserByPasswordAsync(requestDto)
            ?? throw new InvalidOperationException("Unable to get response DTO");

        if (string.IsNullOrEmpty(responseDto.AccessToken))
            return false;
        if (responseDto.ExpiresInSeconds <= 0)
            return false;

        userContext.CurrentUser = responseDto;
        return true;
    }

    public async Task<bool> GuestLoginAsync()
    {
        LoginResponseDto responseDto = await httpHandler.GuestLoginAsync()
            ?? throw new InvalidOperationException("Unable to get response DTO");

        if (string.IsNullOrEmpty(responseDto.AccessToken))
            return false;
        if (responseDto.ExpiresInSeconds <= 0)
            return false;

        userContext.CurrentUser = responseDto;
        return true;
    }

    public void Dispose() => httpHandler?.Dispose();
}
