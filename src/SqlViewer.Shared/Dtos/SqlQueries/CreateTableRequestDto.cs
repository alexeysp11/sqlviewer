using System.Text.Json.Serialization;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.Shared.Extensions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Shared.Dtos.SqlQueries;

/// <summary>
/// Request for creating a table.
/// </summary>
public sealed record CreateTableRequestDto
{
    [JsonPropertyName("databaseType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("connectionString")]
    public required string ConnectionString { get; init; }

    [JsonPropertyName("tableName")]
    public required string TableName { get; init; }

    [JsonPropertyName("columns")]
    public required IEnumerable<ColumnInfoDto> Columns { get; init; }

    /// <summary>
    /// Convert <see cref="Columns"/> field to the collection of <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <returns>Collection of <see cref="VelocipedeColumnInfo"/>.</returns>
    public IEnumerable<VelocipedeColumnInfo> GetVelocipedeColumnInfos()
        => Columns.GetVelocipedeColumnInfos(DatabaseType);
}
