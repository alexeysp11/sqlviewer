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
    SecurityService.SecurityServiceClient securityClient,
    LoginMapper mapper) : ControllerBase
{
    [HttpPost(RestApiPaths.Auth.Login)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        LoginRequest grpcRequest = mapper.MapToLoginRequest(request);
        LoginResponse response = await securityClient.LoginAsync(grpcRequest);
        return Ok(mapper.MapToDto(response));
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
