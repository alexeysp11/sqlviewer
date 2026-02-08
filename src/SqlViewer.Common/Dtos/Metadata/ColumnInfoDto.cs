using System.Data;
using System.Text.Json.Serialization;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Column info DTO.
/// </summary>
public record ColumnInfoDto
{
    /// <summary>
    /// Column name.
    /// </summary>
    [JsonPropertyName("columnName")]
    public required string ColumnName { get; init; }

    /// <summary>
    /// The type of the column.
    /// </summary>
    [JsonPropertyName("columnType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required DbType ColumnType { get; init; }

    /// <summary>
    /// If native column type identifies a character or bit string type, the declared
    /// maximum length; null for all other data types or if no maximum length was declared.
    /// </summary>
    [JsonPropertyName("charMaxLength")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? CharMaxLength { get; init; }

    /// <summary>
    /// Numeric precision for integer/decimal/numeric.
    /// </summary>
    [JsonPropertyName("numericPrecision")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumericPrecision { get; init; }

    /// <summary>
    /// Numeric scale for integer/decimal/numeric.
    /// </summary>
    [JsonPropertyName("numericScale")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumericScale { get; init; }

    /// <summary>
    /// Default value of the column.
    /// </summary>
    [JsonPropertyName("defaultValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? DefaultValue { get; init; }

    /// <summary>
    /// Whether the column is a primary key.
    /// </summary>
    [JsonPropertyName("isPrimaryKey")]
    public bool IsPrimaryKey { get; init; }

    /// <summary>
    /// Whether the column is nullable.
    /// </summary>
    [JsonPropertyName("isNullable")]
    public bool IsNullable { get; init; }

    /// <summary>
    /// Convert DTO to <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <param name="databaseType">Specified database type.</param>
    /// <returns>Instance of <see cref="VelocipedeColumnInfo"/>.</returns>
    public VelocipedeColumnInfo ToVelocipedeColumnInfo(VelocipedeDatabaseType databaseType)
    {
        return new()
        {
            DatabaseType = databaseType,
            ColumnName = ColumnName,
            ColumnType = ColumnType,
            CharMaxLength = CharMaxLength,
            NumericPrecision = NumericPrecision,
            NumericScale = NumericScale,
            DefaultValue = DefaultValue,
            IsPrimaryKey = IsPrimaryKey,
            IsNullable = IsNullable,
        };
    }
}
