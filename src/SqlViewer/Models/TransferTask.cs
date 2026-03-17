namespace SqlViewer.Models;

public record TransferTask(
    string CorrelationId,
    string TableName,
    string SourceDb,
    string TargetDb,
    DateTime StartTime,
    double ProgressPercentage,
    string Status,
    int RowsProcessed,
    string StatusMessage
);
