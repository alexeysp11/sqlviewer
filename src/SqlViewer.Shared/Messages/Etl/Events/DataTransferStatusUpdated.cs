namespace SqlViewer.Shared.Messages.Etl.Events;

public sealed record DataTransferStatusUpdated(
    Guid MessageId,
    Guid CorrelationId,
    string TransferStatus,
    string InternalStatus,
    DateTime Timestamp,
    string? ErrorMessage
);
