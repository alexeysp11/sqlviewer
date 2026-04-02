using System.Text.Json.Serialization;

namespace SqlViewer.Shared.Dtos.Docs;

/// <summary>
/// Response for getting database provider docs.
/// </summary>
public sealed record SqlViewerDocsResponseDto
{
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }
}
