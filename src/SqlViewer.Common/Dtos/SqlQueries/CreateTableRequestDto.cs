using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for creating a table.
/// </summary>
public sealed class CreateTableRequestDto : BaseSqlViewerRequestDto
{
    public required string TableName { get; init; }
    public required IEnumerable<VelocipedeColumnInfo> Columns { get; init; }
}
