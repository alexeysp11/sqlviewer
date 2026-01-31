using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.QueryBuilder;

public sealed class QueryBuilderResponseDto
{
    [JsonPropertyName("query")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Query { get; init; }
}
