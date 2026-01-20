using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Request for getting metadata.
/// </summary>
public sealed class MetadataRequestDto : BaseSqlViewerRequestDto
{
    [JsonPropertyName("tableName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TableName { get; init; }
}
