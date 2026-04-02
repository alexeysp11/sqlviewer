using System.Text.Json.Serialization;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Shared.Dtos.SqlQueries;

/// <summary>
/// Request for droping a table.
/// </summary>
public sealed record DropTableRequestDto
{
    [JsonPropertyName("databaseType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("connectionString")]
    public required string ConnectionString { get; init; }

    [JsonPropertyName("tableName")]
    public required string TableName { get; init; }
}
