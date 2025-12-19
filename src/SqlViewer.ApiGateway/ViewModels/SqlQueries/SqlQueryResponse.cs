using SqlViewer.ApiGateway.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed class SqlQueryResponse : BaseSqlViewerResponse
{
    public List<dynamic>? QueryResult { get; init; }
}
