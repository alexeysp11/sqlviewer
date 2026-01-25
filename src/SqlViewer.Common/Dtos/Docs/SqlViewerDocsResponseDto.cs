using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.Docs;

/// <summary>
/// Response for getting database provider docs.
/// </summary>
public sealed class SqlViewerDocsResponseDto
{
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }
}
