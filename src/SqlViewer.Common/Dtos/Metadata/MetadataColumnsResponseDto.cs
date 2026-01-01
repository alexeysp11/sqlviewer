using SqlViewer.Common.Extensions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Response for getting metadata about coluns.
/// </summary>
public sealed class MetadataColumnsResponseDto : BaseSqlViewerResponseDto
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string? TableName { get; init; }
    public IEnumerable<ColumnInfoDto>? Columns { get; init; }

    /// <summary>
    /// Convert <see cref="Columns"/> field to the collection of <see cref="VelocipedeColumnInfo"/>.
    /// </summary>
    /// <returns>Collection of <see cref="VelocipedeColumnInfo"/>.</returns>
    public IEnumerable<VelocipedeColumnInfo>? GetVelocipedeColumnInfos()
        => Columns?.GetVelocipedeColumnInfos(DatabaseType);
}
