using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Mappings;
using SqlViewer.Security;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Auth;
using LoginRequest = SqlViewer.Security.LoginRequest;

namespace SqlViewer.ApiGateway.Controllers.Security;

[ApiController]
public sealed class AuthApiController(
    ILogger<AuthApiController> logger,
    SecurityService.SecurityServiceClient securityClient,
    LoginMapper mapper) : ControllerBase
{
    [HttpPost(RestApiPaths.Auth.Login)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            LoginRequest grpcRequest = mapper.MapToLoginRequest(request);
            LoginResponse response = await securityClient.LoginAsync(grpcRequest);
            return Ok(mapper.MapToDto(response));
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to log in: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost(RestApiPaths.Auth.LoginAsGuest)]
    public async Task<ActionResult<LoginResponseDto>> LoginGuest()
    {
        try
        {
            LoginResponse response = await securityClient.LoginGuestAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return Ok(mapper.MapToDto(response));
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to log in as guest: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost(RestApiPaths.Auth.RefreshAccessToken)]
    public async Task<ActionResult<LoginResponseDto>> RefreshAccessToken([FromBody] RefreshRequest request)
    {
        try
        {
            LoginResponse response = await securityClient.RefreshAccessTokenAsync(new RefreshAccessTokenRequest
            {
                AccessToken = request.RefreshToken
            });
            return Ok(mapper.MapToDto(response));
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to refresh access token: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
