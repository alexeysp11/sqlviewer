using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Core.Services;

/// <summary>
/// Provides an abstraction for storing incoming messages in the Inbox table.
/// This helps to decouple the Kafka Consumer from specific Database Contexts.
/// </summary>
public interface IInboxService
{
    /// <summary>
    /// Persists a message to the Inbox. 
    /// Should handle idempotency (e.g., checking if CorrelationId already exists).
    /// </summary>
    /// <param name="message">The message entity to store.</param>
    /// <param name="ct">Cancellation token.</param>
    Task StoreMessageAsync(InboxMessageEntity message, CancellationToken ct);
}
