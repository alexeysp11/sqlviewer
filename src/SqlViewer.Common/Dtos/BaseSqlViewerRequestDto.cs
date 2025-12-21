using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Dtos;

/// <summary>
/// Abstract base request.
/// </summary>
public abstract class BaseSqlViewerRequestDto
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
}
