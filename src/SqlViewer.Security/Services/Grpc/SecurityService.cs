using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace SqlViewer.Security.Services.Grpc;

public class SecurityService(ILogger<SecurityService> logger) : Security.SecurityService.SecurityServiceBase
{
    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        return new LoginResponse();
    }

    public override async Task<LoginResponse> LoginGuest(Empty request, ServerCallContext context)
    {
        return new LoginResponse();
    }

    public override async Task<LoginResponse> RefreshAccessToken(RefreshAccessTokenRequest request, ServerCallContext context)
    {
        return new LoginResponse();
    }
}