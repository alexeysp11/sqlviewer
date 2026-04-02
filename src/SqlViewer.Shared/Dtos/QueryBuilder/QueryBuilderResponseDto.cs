using System.Text.Json.Serialization;

namespace SqlViewer.Shared.Dtos.QueryBuilder;

public sealed record QueryBuilderResponseDto
{
    [JsonPropertyName("query")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Query { get; init; }
}
