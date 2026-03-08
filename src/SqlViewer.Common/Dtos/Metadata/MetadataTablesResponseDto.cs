using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Response for getting metadata about tables.
/// </summary>
public sealed record MetadataTablesResponseDto
{
    [JsonPropertyName("tables")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? Tables { get; init; }
}
