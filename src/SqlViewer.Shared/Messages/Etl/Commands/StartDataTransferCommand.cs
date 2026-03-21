namespace SqlViewer.Shared.Messages.Etl.Commands;

public sealed record StartDataTransferCommand(
    Guid CorrelationId,
    string SourceConnectionString,
    string TargetConnectionString,
    string TableName
);
