using SqlViewer.Shared.Dtos.Auth;

namespace SqlViewer.StorageContexts.Abstractions;

#nullable enable

/// <summary>
/// Used to store the context of the authorized user within the current session.
/// </summary>
public interface IUserContext
{
    LoginResponseDto? CurrentUser { get; set; }

    bool IsAuthenticated => CurrentUser != null;

    string? TokenType => CurrentUser?.TokenType;
    string? AccessToken => CurrentUser?.AccessToken;
    string? RefreshToken => CurrentUser?.RefreshToken;
}
