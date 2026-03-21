using System.Text.Json.Serialization;

namespace SqlViewer.Shared.Dtos.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed record SqlQueryResponseDto
{
    [JsonPropertyName("queryResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<dynamic>? QueryResult { get; init; }
}
