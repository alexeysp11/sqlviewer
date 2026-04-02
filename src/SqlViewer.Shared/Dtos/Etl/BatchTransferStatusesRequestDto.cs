namespace SqlViewer.Shared.Dtos.Etl;

public sealed record BatchTransferStatusesRequestDto
{
    /// <summary>
    /// Unique identifier of the user requesting statuses.
    /// </summary>
    public Guid UserUid { get; init; }

    /// <summary>
    /// List of Correlation IDs to track.
    /// </summary>
    public IReadOnlyCollection<Guid> CorrelationIds { get; init; } = [];
}
