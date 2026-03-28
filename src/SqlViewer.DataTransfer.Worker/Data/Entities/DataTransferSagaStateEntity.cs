using SqlViewer.Etl.Core.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.DataTransfer.Worker.Data.Entities;

/// <summary>
/// Persistent state of the Data Transfer Saga.
/// </summary>
public class DataTransferSagaStateEntity
{
    public long Id { get; set; }
    public required Guid CorrelationId { get; set; }
    public required TransferSagaStatus CurrentState { get; set; } = TransferSagaStatus.Initial;
    public required string SourceConnectionString { get; set; }
    public required string TargetConnectionString { get; set; }
    public required VelocipedeDatabaseType SourceDatabaseType { get; set; }
    public required VelocipedeDatabaseType TargetDatabaseType { get; set; }
    public required string TableName { get; set; }
    public required string UserUid { get; set; }
    public int RowsProcessed { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}
