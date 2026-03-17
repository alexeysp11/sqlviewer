namespace SqlViewer.Common.Messages.Etl.Commands;

public sealed record StartDataTransferCommand(
    Guid CorrelationId,
    string SourceConnectionString,
    string TargetConnectionString,
    string TableName
);
