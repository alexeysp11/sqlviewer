using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed class SqlQueryResponseDto : BaseSqlViewerResponseDto
{
    [JsonPropertyName("queryResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<dynamic>? QueryResult { get; init; }
}
