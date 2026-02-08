using SqlViewer.Common.Enums;

namespace SqlViewer.ApiGateway.VerticalSlices.Security.Services;

public interface ITokenService
{
    string GenerateAccessToken(string username, SqlViewerAuthRole role);
    string GenerateRefreshToken();
}
