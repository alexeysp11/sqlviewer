using SqlViewer.Common.Dtos.Auth;

namespace SqlViewer.StorageContexts;

#nullable enable

public interface IUserContext
{
    LoginResponseDto? CurrentUser { get; set; }
    
    bool IsAuthenticated => CurrentUser != null;

    string? TokenType => CurrentUser?.TokenType;
    string? AccessToken => CurrentUser?.AccessToken;
    string? RefreshToken => CurrentUser?.RefreshToken;
}
