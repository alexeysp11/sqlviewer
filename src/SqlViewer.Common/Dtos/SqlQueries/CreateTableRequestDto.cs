using SqlViewer.Common.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for creating a table.
/// </summary>
public sealed class CreateTableRequestDto : BaseSqlViewerRequestDto
{
    public required string TableName { get; init; }
    public required IEnumerable<ColumnInfoDto> Columns { get; init; }

    /// <summary>
    /// Convert <see cref="Columns"/> field to the collection of <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <returns>Collection of <see cref="VelocipedeColumnInfo"/>.</returns>
    public IEnumerable<VelocipedeColumnInfo> GetVelocipedeColumnInfos()
    {
        List<VelocipedeColumnInfo> result = [];
        foreach (ColumnInfoDto column in Columns)
        {
            result.Add(column.ToVelocipedeColumnInfo(DatabaseType));
        }
        return result;
    }
}
