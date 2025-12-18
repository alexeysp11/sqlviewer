using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Request for creating a table.
/// </summary>
public sealed class CreateTableRequest
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string TableName { get; init; }
    public required IEnumerable<VelocipedeColumnInfo> Columns { get; init; }
    public required string ConnectionString { get; init; }
}
