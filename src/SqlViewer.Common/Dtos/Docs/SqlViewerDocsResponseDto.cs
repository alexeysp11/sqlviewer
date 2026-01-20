using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.Docs;

/// <summary>
/// Response for getting database provider docs.
/// </summary>
public sealed class SqlViewerDocsResponseDto : BaseSqlViewerResponseDto
{
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }
}
