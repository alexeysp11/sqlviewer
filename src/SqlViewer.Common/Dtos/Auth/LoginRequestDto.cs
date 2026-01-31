using System.Text.Json.Serialization;
using SqlViewer.Common.Enums;

namespace SqlViewer.Common.Dtos.Auth;

/// <summary>
/// Request DTO for verifying user credentials.
/// </summary>
public sealed class LoginRequestDto
{
    [JsonPropertyName("authType")]
    public required SqlViewerAuthType AuthType { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Password { get; init; }
}
