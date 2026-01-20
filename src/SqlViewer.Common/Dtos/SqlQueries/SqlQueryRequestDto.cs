using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for executing SQL query.
/// </summary>
public sealed class SqlQueryRequestDto : BaseSqlViewerRequestDto
{
    [JsonPropertyName("query")]
    public required string Query { get; init; }
}
