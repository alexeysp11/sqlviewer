using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.QueryBuilder;

public sealed class QueryBuilderResponseDto : BaseSqlViewerResponseDto
{
    [JsonPropertyName("query")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Query { get; init; }
}
