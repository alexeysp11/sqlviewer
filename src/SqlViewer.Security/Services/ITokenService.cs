using SqlViewer.Common.Enums;

namespace SqlViewer.Security.Services;

public interface ITokenService
{
    string GenerateAccessToken(string username, SqlViewerAuthRole role);
    string GenerateRefreshToken();
}
