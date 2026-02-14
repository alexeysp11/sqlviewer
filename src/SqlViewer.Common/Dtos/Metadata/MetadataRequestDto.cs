using System.Text.Json.Serialization;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Request for getting metadata.
/// </summary>
public sealed record MetadataRequestDto
{
    [JsonPropertyName("databaseType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("connectionString")]
    public required string ConnectionString { get; init; }

    [JsonPropertyName("tableName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TableName { get; init; }
}
