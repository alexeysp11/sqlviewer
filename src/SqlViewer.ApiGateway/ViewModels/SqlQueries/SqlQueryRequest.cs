using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Request for executing SQL query.
/// </summary>
public sealed class SqlQueryRequest
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string Query { get; init; }
    public required string ConnectionString { get; init; }
}
