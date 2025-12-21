namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for droping a table.
/// </summary>
public sealed class DropTableRequestDto : BaseSqlViewerRequestDto
{
    public required string TableName { get; init; }
}
