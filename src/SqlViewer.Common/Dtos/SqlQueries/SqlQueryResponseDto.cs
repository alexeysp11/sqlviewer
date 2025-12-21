namespace SqlViewer.Common.Dtos.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed class SqlQueryResponseDto : BaseSqlViewerResponseDto
{
    public List<dynamic>? QueryResult { get; init; }
}
