using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
public sealed class AuthApiController(
    ILogger<MetadataApiController> logger,
    IAuthService authService) : ControllerBase
{
    [HttpPost]
    [Route(RestApiPaths.Auth.Login)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            bool success = request.AuthType switch
            {
                SqlViewerAuthType.ByPassword => await authService.VilidateByPasswordAsync(request.Username, request.Password),
                _ => throw new NotSupportedException($"Specified authentication type is not supported: {request.AuthType}")
            };
            if (!success)
            {
                return Unauthorized("Authentication failed");
            }

            LoginResponseDto response = await authService.CreateSessionAsync(request.Username);

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Auth.LoginAsGuest)]
    public ActionResult<LoginResponseDto> LoginGuest()
    {
        try
        {
            LoginResponseDto response = authService.CreateGuestSession();
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Auth.RefreshAccessToken)]
    public async Task<ActionResult<LoginResponseDto>> RefreshAccessToken([FromBody] RefreshRequest request)
    {
        try
        {
            LoginResponseDto response = await authService.RefreshSessionAsync(request.RefreshToken);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
