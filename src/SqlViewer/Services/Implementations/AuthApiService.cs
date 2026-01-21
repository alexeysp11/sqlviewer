using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class AuthApiService(IHttpHandler httpHandler) : IAuthApiService
{
    public async Task<bool> VerifyUserByPasswordAsync(string username, string password)
    {
        LoginRequestDto requestDto = new()
        {
            AuthType = SqlViewerAuthType.ByPassword,
            Username = username,
            Password = password
        };

        LoginResponseDto responseDto = await httpHandler.VerifyUserByPasswordAsync(requestDto);

        //if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
        //    throw new InvalidOperationException("Unable to get response DTO");
        //if (responseDto.Status is SqlOperationStatus.Failed)
        //    throw new InvalidOperationException(responseDto.ErrorMessage);

        //return responseDto.Status is SqlOperationStatus.Success;

        return await Task.FromResult(responseDto is not null);
    }

    public async Task<bool> GuestLoginAsync()
    {
        LoginResponseDto responseDto = await httpHandler.GuestLoginAsync();

        //if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
        //    throw new InvalidOperationException("Unable to get response DTO");
        //if (responseDto.Status is SqlOperationStatus.Failed)
        //    throw new InvalidOperationException(responseDto.ErrorMessage);

        //return responseDto.Status is SqlOperationStatus.Success;

        return await Task.FromResult(responseDto is not null);
    }

    public void Dispose() => httpHandler?.Dispose();
}
