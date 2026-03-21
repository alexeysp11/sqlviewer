namespace SqlViewer.Shared.Messages.Etl.Events;

/// <summary>
/// An event published by the Worker service when a critical error occurs.
/// </summary>
public sealed record DataTransferFailed(
    Guid CorrelationId,
    string ErrorMessage,
    string? ExceptionType,
    string? StackTrace,
    DateTime FailedAt
);
