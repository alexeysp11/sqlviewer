namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Request for executing SQL query.
/// </summary>
public sealed class SqlQueryRequest : BaseSqlViewerRequest
{
    public required string Query { get; init; }
}
