using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Shared.Dtos.Etl;

public sealed record TransferJobDto
{
    public required Guid CorrelationId { get; init; }
    public required string SourceConnectionString { get; init; }
    public required string TargetConnectionString { get; init; }
    public required VelocipedeDatabaseType SourceDatabaseType { get; init; }
    public required VelocipedeDatabaseType TargetDatabaseType { get; init; }
    public required string TableName { get; init; }
    public required string Status { get; init; }
    public DateTime Time { get; init; }
}
