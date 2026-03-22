namespace SqlViewer.Shared.Dtos.Etl;

public sealed record TransferHistoryResponseDto
{
    public List<TransferJobDto> Items { get; init; } = [];
    public Guid? CursorCorrelationId { get; init; }
}
