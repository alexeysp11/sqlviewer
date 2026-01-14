using SqlViewer.Common.Enums;

namespace SqlViewer.Common.Dtos.Auth;

/// <summary>
/// Request DTO for verifying user credentials.
/// </summary>
public sealed class LoginRequestDto
{
    public required SqlViewerAuthType AuthType { get; init; }
    public required string Username { get; init; }
    public string? Password { get; init; }
}
