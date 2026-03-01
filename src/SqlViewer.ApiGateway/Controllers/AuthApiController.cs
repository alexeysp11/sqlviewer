using Grpc.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Mappings;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Security;
using LoginRequest = SqlViewer.Security.LoginRequest;

namespace SqlViewer.ApiGateway.Controllers;

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
        catch (RpcException ex)
        {
            logger.LogWarning("gRPC Error: {StatusDetail}, Code: {StatusCode}", ex.Status.Detail, ex.StatusCode);
            return ex.StatusCode switch
            {
                Grpc.Core.StatusCode.Unauthenticated => Unauthorized(new { message = "Incorrect login or password" }),
                Grpc.Core.StatusCode.InvalidArgument => BadRequest(new { message = ex.Status.Detail }),
                Grpc.Core.StatusCode.PermissionDenied => Forbid(),
                Grpc.Core.StatusCode.NotFound => NotFound(new { message = ex.Status.Detail }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error on the authorization service side" })
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in API Gateway");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost(RestApiPaths.Auth.LoginAsGuest)]
    public async Task<ActionResult<LoginResponseDto>> LoginGuest()
    {
        LoginResponse response = await securityClient.LoginGuestAsync(new Google.Protobuf.WellKnownTypes.Empty());
        return Ok(mapper.MapToDto(response));
    }

    [HttpPost(RestApiPaths.Auth.RefreshAccessToken)]
    public async Task<ActionResult<LoginResponseDto>> RefreshAccessToken([FromBody] RefreshRequest request)
    {
        LoginResponse response = await securityClient.RefreshAccessTokenAsync(new RefreshAccessTokenRequest
        {
            AccessToken = request.RefreshToken
        });
        return Ok(mapper.MapToDto(response));
    }
}
