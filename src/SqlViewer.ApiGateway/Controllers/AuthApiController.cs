using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;
using SqlViewer.Security;
using AuthType = SqlViewer.Security.AuthType;
using LoginRequest = SqlViewer.Security.LoginRequest;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
public sealed class AuthApiController(
    ILogger<AuthApiController> logger,
    SecurityService.SecurityServiceClient securityClient) : ControllerBase
{
    [HttpPost(RestApiPaths.Auth.Login)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            LoginRequest grpcRequest = new()
            {
                Username = request.Username,
                Password = request.Password,
                AuthType = (AuthType)request.AuthType
            };

            LoginResponse response = await securityClient.LoginAsync(grpcRequest);
            return Ok(MapToDto(response));
        }
        catch (Exception ex)
        {
            logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost(RestApiPaths.Auth.LoginAsGuest)]
    public async Task<ActionResult<LoginResponseDto>> LoginGuest()
    {
        LoginResponse response = await securityClient.LoginGuestAsync(new Google.Protobuf.WellKnownTypes.Empty());
        return Ok(MapToDto(response));
    }

    [HttpPost(RestApiPaths.Auth.RefreshAccessToken)]
    public async Task<ActionResult<LoginResponseDto>> RefreshAccessToken([FromBody] RefreshRequest request)
    {
        LoginResponse response = await securityClient.RefreshAccessTokenAsync(new RefreshAccessTokenRequest
        {
            AccessToken = request.RefreshToken
        });
        return Ok(MapToDto(response));
    }

    private static LoginResponseDto MapToDto(LoginResponse resp) => new()
    {
        AccessToken = resp.AccessToken,
        RefreshToken = resp.RefreshToken,
        TokenType = resp.TokenType,
        ExpiresInSeconds = resp.ExpiresInSeconds,
        Username = resp.Username,
        Role = (SqlViewerAuthRole)resp.Role
    };
}
