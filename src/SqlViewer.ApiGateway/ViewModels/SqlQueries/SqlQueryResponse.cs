using SqlViewer.ApiGateway.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed class SqlQueryResponse
{
    public required SqlOperationStatus Status { get; init; }
    public List<dynamic>? QueryResult { get; init; }
    public string? ErrorMessage { get; init; }
}
