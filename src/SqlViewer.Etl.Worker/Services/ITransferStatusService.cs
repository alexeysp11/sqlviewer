using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Services;

public interface ITransferStatusService
{
    /// <summary>
    /// The type of message this handler can process
    /// </summary>
    string MessageType { get; }

    /// <summary>
    /// Core logic for processing the inbox message
    /// </summary>
    Task ProcessAsync(InboxMessageEntity message, CancellationToken ct);
}
