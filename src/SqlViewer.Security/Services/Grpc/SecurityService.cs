using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Security.Domain.Identities;

namespace SqlViewer.Security.Services.Grpc;

public class SecurityGrpcService(
    ILogger<SecurityGrpcService> logger,
    IIdentityManager identityManager) : SecurityService.SecurityServiceBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        try
        {
            bool success = request.AuthType switch
            {
                AuthType.Password => await identityManager.VilidateByPasswordAsync(request.Username, request.Password),
                _ => throw new RpcException(new Status(StatusCode.InvalidArgument, $"Unsupported auth type: {request.AuthType}"))
            };

            if (!success)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Authentication failed"));

            LoginResponseDto result = await identityManager.CreateSessionAsync(request.Username);
            return MapToResponse(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during Login");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override Task<LoginResponse> LoginGuest(Empty request, ServerCallContext context)
    {
        LoginResponseDto result = identityManager.CreateGuestSession();
        return Task.FromResult(MapToResponse(result));
    }

    public override async Task<LoginResponse> RefreshAccessToken(RefreshAccessTokenRequest request, ServerCallContext context)
    {
        try
        {
            LoginResponseDto result = await identityManager.RefreshSessionAsync(request.AccessToken);
            return MapToResponse(result);
        }
        catch (UnauthorizedAccessException)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token"));
        }
    }

    private static LoginResponse MapToResponse(LoginResponseDto dto) => new()
    {
        AccessToken = dto.AccessToken,
        RefreshToken = dto.RefreshToken ?? "",
        TokenType = dto.TokenType,
        ExpiresInSeconds = dto.ExpiresInSeconds,
        Username = dto.Username ?? "",
        Role = (AuthRole)dto.Role
    };
}
