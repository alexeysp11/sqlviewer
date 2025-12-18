using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Request for droping a table.
/// </summary>
public sealed class DropTableRequest
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string TableName { get; init; }
    public required string ConnectionString { get; init; }
}
