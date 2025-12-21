namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Request for executing SQL query.
/// </summary>
public sealed class SqlQueryRequestDto : BaseSqlViewerRequestDto
{
    public required string Query { get; init; }
}
