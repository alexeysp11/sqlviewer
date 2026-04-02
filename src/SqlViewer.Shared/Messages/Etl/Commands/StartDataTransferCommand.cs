using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Shared.Messages.Etl.Commands;

public sealed record StartDataTransferCommand(
    Guid CorrelationId,
    string SourceConnectionString,
    string TargetConnectionString,
    VelocipedeDatabaseType SourceDatabaseType,
    VelocipedeDatabaseType TargetDatabaseType,
    string TableName,
    string UserUid
);
