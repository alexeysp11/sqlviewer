namespace SqlViewer.Shared.Dtos.Etl;

public class StartTransferRequestDto
{
    public required string SourceConnectionString { get; init; }
    public required string TargetConnectionString { get; init; }
    public required string TableName { get; init; }
    public required string UserUid { get; init; }
}
