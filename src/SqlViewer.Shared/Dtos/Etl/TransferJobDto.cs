namespace SqlViewer.Shared.Dtos.Etl;

public sealed record TransferJobDto
{
    public required Guid CorrelationId { get; init; }
    public required string Source { get; init; }
    public required string Target { get; init; }
    public required string Status { get; init; }
    public DateTime Time { get; init; }
}
