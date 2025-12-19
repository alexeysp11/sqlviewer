using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Request for droping a table.
/// </summary>
public sealed class DropTableRequest : BaseSqlViewerRequest
{
    public required string TableName { get; init; }
}
