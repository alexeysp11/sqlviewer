using System.Text.Json.Serialization;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for executing SQL query.
/// </summary>
public sealed record SqlQueryRequestDto
{
    [JsonPropertyName("databaseType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("connectionString")]
    public required string ConnectionString { get; init; }

    [JsonPropertyName("query")]
    public required string Query { get; init; }
}
