using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Security.Domain.Identities;
using SqlViewer.Security.Mappings;

namespace SqlViewer.Security.Services.Grpc;

public class SecurityGrpcService(
    ILogger<SecurityGrpcService> logger,
    IIdentityManager identityManager,
    SecurityServiceMapper mapper) : SecurityService.SecurityServiceBase
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
                throw new RpcException(new Status(StatusCode.Unauthenticated, string.Empty));

            LoginResponseDto result = await identityManager.CreateSessionAsync(request.Username);
            return mapper.MapToGrpc(result);
        }
        catch (RpcException)
        {
            throw;
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
        return Task.FromResult(mapper.MapToGrpc(result));
    }

    public override async Task<LoginResponse> RefreshAccessToken(RefreshAccessTokenRequest request, ServerCallContext context)
    {
        try
        {
            LoginResponseDto result = await identityManager.RefreshSessionAsync(request.AccessToken);
            return mapper.MapToGrpc(result);
        }
        catch (UnauthorizedAccessException)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token"));
        }
    }
}
