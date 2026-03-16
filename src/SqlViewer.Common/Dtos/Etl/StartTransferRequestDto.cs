namespace SqlViewer.Common.Dtos.Etl;

public class StartTransferRequestDto
{
    public required string SourceConnectionString { get; init; }
    public required string TargetConnectionString { get; init; }
    public required string TableName { get; init; }
}
