namespace SqlViewer.Shared.Dtos.Etl;

public sealed record TransferStatusResponseDto
{
    public Guid CorrelationId { get; init; }
    public double Progress { get; init; }
    public string? StatusMessage { get; init; }
    public bool IsFinalState { get; init; }
}
