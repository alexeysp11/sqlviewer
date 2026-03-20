namespace SqlViewer.Etl.Core.Data.Entities;

/// <summary>
/// Persistent state of the Data Transfer Saga.
/// </summary>
public class DataTransferSagaStateEntity
{
    public required Guid CorrelationId { get; set; }
    public required string CurrentState { get; set; } = "Initial"; // Initial, Validating, CommandSent, InProgress, Completed, Faulted
    public required string SourceDb { get; set; }
    public required string TargetDb { get; set; }
    public int RowsProcessed { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}
