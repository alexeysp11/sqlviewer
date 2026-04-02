using System.Data;
using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Shared.Extensions;

public static class VelocipedeNativeColumnInfoExtensions
{
    /// <summary>
    /// Get column info DTOs from <see cref="VelocipedeNativeColumnInfo"/> list.
    /// </summary>
    /// <param name="columns">Original <see cref="VelocipedeNativeColumnInfo"/> list.</param>
    /// <returns>Collection of <see cref="ColumnInfoDto"/>.</returns>
    /// <exception cref="InvalidDataException">Thrown when column name is null or empty, or calculated column type is null.</exception>
    public static IEnumerable<ColumnInfoResponseDto> GetColumnInfoDtos(this List<VelocipedeNativeColumnInfo> columns)
    {
        List<ColumnInfoResponseDto> result = [];
        foreach (VelocipedeNativeColumnInfo column in columns)
        {
            if (string.IsNullOrEmpty(column.ColumnName))
            {
                throw new InvalidDataException("Column name could not be null or empty");
            }
            if (column.CalculatedColumnType is null)
            {
                throw new InvalidDataException("Calculated column type could not be null");
            }

            result.Add(new()
            {
                ColumnName = column.ColumnName,
                ColumnType = (DbType)column.CalculatedColumnType,
                NativeColumnType = column.NativeColumnType,
                CharMaxLength = column.CharMaxLength,
                NumericPrecision = column.NumericPrecision,
                NumericScale = column.NumericScale,
                DefaultValue = column.DefaultValue,
                IsPrimaryKey = column.IsPrimaryKey,
                IsNullable = column.IsNullable,
            });
        }
        return result;
    }
}
