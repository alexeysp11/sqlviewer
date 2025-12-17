using SqlViewer.ApiGateway.Enums;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Factories;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Response for executing SQL query.
/// </summary>
public sealed class SqlQueryExecuteResponse
{
    public required SqlQueryExecuteStatus Status { get; init; }
    public List<dynamic>? QueryResult { get; init; }
    public string? ErrorMessage { get; init; }
}
