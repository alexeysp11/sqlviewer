namespace SqlViewer.Common.Messages.Etl.Events;

public sealed record DataTransferCompleted(
    Guid CorrelationId,
    int TotalRows,
    DateTime FinishedAt
);
