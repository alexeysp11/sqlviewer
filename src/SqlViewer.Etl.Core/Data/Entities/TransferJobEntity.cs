using SqlViewer.Etl.Core.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Core.Data.Entities;

public sealed class TransferJobEntity
{
    public long Id { get; set; }
    public required Guid CorrelationId { get; set; }
    public required Guid UserUid { get; set; }
    public required string SourceConnectionString { get; set; }
    public required string TargetConnectionString { get; set; }
    public required VelocipedeDatabaseType SourceDatabaseType { get; init; }
    public required VelocipedeDatabaseType TargetDatabaseType { get; init; }
    public TransferStatus CurrentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<TransferStatusLogEntity> Logs { get; set; } = [];
}
