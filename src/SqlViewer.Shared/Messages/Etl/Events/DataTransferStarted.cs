namespace SqlViewer.Shared.Messages.Etl.Events;

public sealed record DataTransferStarted(
    Guid CorrelationId,
    string SourceDbName,
    string TargetDbName,
    long TotalRowsEstimated, // Useful for initializing ProgressBar.
    DateTime StartedAt
);
