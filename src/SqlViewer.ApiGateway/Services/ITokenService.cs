using SqlViewer.Common.Enums;

namespace SqlViewer.ApiGateway.Services;

public interface ITokenService
{
    string GenerateAccessToken(string username, SqlViewerAuthRole role);
    string GenerateRefreshToken();
}
