namespace SqlViewer.Shared.Messages.Storage.Entities;

/// <summary>
/// Represents a message to be sent to a message broker, stored within a local transaction.
/// </summary>
public class OutboxMessageEntity
{
    /// <summary>
    /// Unique identifier for the record.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// User UID.
    /// </summary>
    public Guid? UserUid { get; set; }

    /// <summary>
    /// Business operation identifier to track the message flow across services.
    /// </summary>
    public required Guid CorrelationId { get; set; }

    /// <summary>
    /// The Kafka topic where the message should be published.
    /// </summary>
    public required string Topic { get; set; }

    /// <summary>
    /// The type name used by the consumer to identify the payload structure.
    /// </summary>
    public required string MessageType { get; set; }

    /// <summary>
    /// The serialized message content (JSON).
    /// </summary>
    public required string Payload { get; set; }

    /// <summary>
    /// Contains timestamp once created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Null if not sent; contains timestamp once confirmed by the broker.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Details about delivery failures for troubleshooting.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// The number of failed attempts to send a message to the message broker.
    /// Used to prevent endless processing of "poison messages".
    /// When the value reaches the maximum allowed threshold, the message is excluded from automatic processing
    /// for manual analysis of the cause of the error.
    /// </summary>
    public int RetryCount { get; set; }
}
