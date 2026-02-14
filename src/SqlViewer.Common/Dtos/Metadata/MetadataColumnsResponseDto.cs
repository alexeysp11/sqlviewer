using System.Text.Json.Serialization;
using SqlViewer.Common.Extensions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Response for getting metadata about coluns.
/// </summary>
public sealed record MetadataColumnsResponseDto
{
    [JsonPropertyName("databaseType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("tableName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string? TableName { get; init; }

    [JsonPropertyName("columns")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ColumnInfoResponseDto>? Columns { get; init; }

    /// <summary>
    /// Convert <see cref="Columns"/> field to the collection of <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <returns>Collection of <see cref="VelocipedeColumnInfo"/>.</returns>
    public IEnumerable<VelocipedeColumnInfo>? GetVelocipedeColumnInfos()
        => Columns?.GetVelocipedeColumnInfos(DatabaseType);
}
