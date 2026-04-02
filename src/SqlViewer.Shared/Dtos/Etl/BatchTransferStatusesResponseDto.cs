namespace SqlViewer.Shared.Dtos.Etl;

public sealed record BatchTransferStatusesResponseDto
{
    /// <summary>
    /// Collection of status items for the requested jobs.
    /// </summary>
    public IReadOnlyCollection<TransferStatusResponseDto> Items { get; init; } = [];
}
