using SqlViewer.Common.Messages.Storage.Enums;

namespace SqlViewer.Common.Messages.Storage.Entities;

/// <summary>
/// Represents a message received from a message broker, stored for idempotent processing.
/// </summary>
public class InboxMessageEntity
{
    /// <summary>
    /// Unique identifier for the record.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Unique identifier for the business operation to ensure "exactly-once" processing.
    /// </summary>
    public required Guid CorrelationId { get; set; }
    
    /// <summary>
    /// The name of the message type (e.g., "StartDataTransferCommand") for deserialization.
    /// </summary>
    public required string MessageType { get; set; }
    
    /// <summary>
    /// The serialized message content (JSON).
    /// </summary>
    public required string Payload { get; set; }
    
    /// <summary>
    /// The current processing status within this service.
    /// </summary>
    public InboxStatus Status { get; set; } = InboxStatus.Received;
    
    /// <summary>
    /// When the message was initially inserted into the database.
    /// </summary>
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// When the message was successfully processed.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    
    /// <summary>
    /// Contains error details if processing fails.
    /// </summary>
    public string? Error { get; set; }
}
