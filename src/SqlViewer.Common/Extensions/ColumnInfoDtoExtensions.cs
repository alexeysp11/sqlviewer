using SqlViewer.Common.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Extensions;

public static class ColumnInfoDtoExtensions
{
    /// <summary>
    /// Convert <paramref name="columns"/> to the collection of <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <returns>Collection of <see cref="VelocipedeColumnInfo"/>.</returns>
    public static IEnumerable<VelocipedeColumnInfo> GetVelocipedeColumnInfos(
        this IEnumerable<ColumnInfoDto> columns,
        VelocipedeDatabaseType databaseType)
    {
        List<VelocipedeColumnInfo> result = [];
        foreach (ColumnInfoDto column in columns)
        {
            result.Add(column.ToVelocipedeColumnInfo(databaseType));
        }
        return result;
    }
}
