using System.ComponentModel.DataAnnotations;
using SqlViewer.DataTransfer.Worker.Enums;

namespace SqlViewer.DataTransfer.Worker.Data.Entities;

/// <summary>
/// Represents the current execution state of a data transfer process within the DataTransfer Worker.
/// </summary>
public sealed class TransferExecutionEntity
{
    /// <summary>
    /// Unique identifier for the execution record.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Correlation ID used to track the request across microservices.
    /// </summary>
    public required Guid CorrelationId { get; set; }

    /// <summary>
    /// Current operational status of the transfer execution.
    /// </summary>
    public TransferExecutionStatus Status { get; set; } = TransferExecutionStatus.Initializing;

    /// <summary>
    /// Transfer progress percentage, ranging from 0.0 to 100.0.
    /// </summary>
    [Range(0.0, 100.0)]
    public double Progress { get; set; }

    /// <summary>
    /// Number of rows successfully processed during the transfer.
    /// </summary>
    public int RowsProcessed { get; set; }

    /// <summary>
    /// Total number of rows expected to be processed.
    /// </summary>
    public int TotalRows { get; set; }

    /// <summary>
    /// Contains technical details or error messages if the execution fails.
    /// </summary>
    public string? LastErrorMessage { get; set; }

    /// <summary>
    /// Timestamp when the execution was initiated.
    /// </summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the execution reached a terminal state (Completed or Failed).
    /// </summary>
    public DateTime? FinishedAt { get; set; }

    /// <summary>
    /// Name of the database table being processed.
    /// </summary>
    public string TableName { get; set; } = string.Empty;
}
