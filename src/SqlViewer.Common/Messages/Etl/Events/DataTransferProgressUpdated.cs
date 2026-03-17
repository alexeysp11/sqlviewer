namespace SqlViewer.Common.Messages.Etl.Events;

public sealed record DataTransferProgressUpdated(
    Guid CorrelationId,
    double Progress,
    string StatusMessage,
    int RowsProcessed,
    DateTime Timestamp
);
