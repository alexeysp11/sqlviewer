using System.Text.Json.Serialization;
using SqlViewer.Shared.Enums;

namespace SqlViewer.Shared.Dtos.Auth;

/// <summary>
/// Request DTO for verifying user credentials.
/// </summary>
public sealed record LoginRequestDto
{
    [JsonPropertyName("authType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SqlViewerAuthType AuthType { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Password { get; init; }
}
