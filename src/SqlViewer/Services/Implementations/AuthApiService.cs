using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class AuthApiService(IHttpHandler httpHandler) : IAuthApiService
{
    private readonly IHttpHandler _httpHandler = httpHandler;

    public async Task<bool> VerifyUserByPasswordAsync(string username, string password)
    {
        LoginRequestDto requestDto = new()
        {
            AuthType = SqlViewerAuthType.ByPassword,
            Username = username,
            Password = password
        };

        CommonResponseDto responseDto = await _httpHandler.VerifyUserByPasswordAsync(requestDto);

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

        return responseDto.Status is SqlOperationStatus.Success;
    }

    public void Dispose() => _httpHandler?.Dispose();
}
