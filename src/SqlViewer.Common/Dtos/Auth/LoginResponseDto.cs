using System.Text.Json.Serialization;
using SqlViewer.Common.Enums;

namespace SqlViewer.Common.Dtos.Auth;

/// <summary>
/// Response DTO for verifying user credentials.
/// </summary>
public sealed class LoginResponseDto
{
    /// <summary>
    /// JWT access token for Header <c>"Authorization: Bearer AccessToken"</c>.
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// Token for refreshing a token pair without re-entering the password.
    /// Can be null for guest mode.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; init; }

    /// <summary>
    /// Token type (always <c>Bearer</c>).
    /// </summary>
    [JsonPropertyName("tokenType")]
    public string TokenType { get; init; } = "Bearer";

    /// <summary>
    /// AccessToken expiration time (in seconds) so that the client knows when it is time to do a Refresh.
    /// </summary>
    [JsonPropertyName("expiresInSeconds")]
    public double ExpiresInSeconds { get; init; }

    /// <summary>
    /// Username.
    /// </summary>
    [JsonPropertyName("username")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Username { get; init; }

    /// <summary>
    /// User authentication role.
    /// </summary>
    [JsonPropertyName("authRole")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SqlViewerAuthRole Role { get; init; }
}
