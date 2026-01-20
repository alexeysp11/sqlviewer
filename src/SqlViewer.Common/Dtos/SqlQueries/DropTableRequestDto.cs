using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for droping a table.
/// </summary>
public sealed class DropTableRequestDto : BaseSqlViewerRequestDto
{
    [JsonPropertyName("tableName")]
    public required string TableName { get; init; }
}
