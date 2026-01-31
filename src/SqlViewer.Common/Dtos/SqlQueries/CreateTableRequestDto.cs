using System.Text.Json.Serialization;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Extensions;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for creating a table.
/// </summary>
public sealed class CreateTableRequestDto : BaseSqlViewerRequestDto
{
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
