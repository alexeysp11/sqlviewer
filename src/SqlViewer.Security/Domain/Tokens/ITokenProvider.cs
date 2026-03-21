using SqlViewer.Shared.Enums;

namespace SqlViewer.Security.Domain.Tokens;

public interface ITokenProvider
{
    string GenerateAccessToken(string username, SqlViewerAuthRole role);
    string GenerateRefreshToken();
}
