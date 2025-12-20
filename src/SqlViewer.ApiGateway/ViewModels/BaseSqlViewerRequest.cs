using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.ViewModels;

/// <summary>
/// Abstract base request.
/// </summary>
public abstract class BaseSqlViewerRequest
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
}
