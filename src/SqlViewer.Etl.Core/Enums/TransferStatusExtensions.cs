namespace SqlViewer.Etl.Core.Enums;

/// <summary>
/// Provides mapping logic between internal Saga statuses and client-facing Transfer statuses.
/// </summary>
public static class TransferStatusExtensions
{
    /// <summary>
    /// Maps internal <see cref="TransferSagaStatus"/> to <see cref="TransferStatus"/> for client applications.
    /// </summary>
    public static TransferStatus ToTransferStatus(this TransferSagaStatus sagaStatus)
    {
        return sagaStatus switch
        {
            TransferSagaStatus.Initial => TransferStatus.Queued,

            TransferSagaStatus.AccessabilityCheck => TransferStatus.Started,

            TransferSagaStatus.SchemaValidation or
            TransferSagaStatus.Transferring => TransferStatus.Progress,

            TransferSagaStatus.Completed => TransferStatus.Completed,

            TransferSagaStatus.Failed or
            TransferSagaStatus.TimedOut or
            TransferSagaStatus.Cancelled => TransferStatus.Failed,

            _ => TransferStatus.None
        };
    }

    /// <summary>
    /// Maps <see cref="TransferStatus"/> back to its primary <see cref="TransferSagaStatus"/> equivalent.
    /// Note: This is a lossy mapping as multiple saga states can map to a single client state.
    /// </summary>
    public static TransferSagaStatus ToSagaStatus(this TransferStatus clientStatus)
    {
        return clientStatus switch
        {
            TransferStatus.Queued => TransferSagaStatus.Initial,
            TransferStatus.Started => TransferSagaStatus.AccessabilityCheck,
            TransferStatus.Progress => TransferSagaStatus.Transferring,
            TransferStatus.Completed => TransferSagaStatus.Completed,
            TransferStatus.Failed => TransferSagaStatus.Failed,
            _ => TransferSagaStatus.Initial
        };
    }

    /// <summary>
    /// Returns a numerical rank for the given <see cref="TransferStatus"/>.
    /// </summary>
    /// <param name="status">The transfer status to evaluate.</param>
    /// <returns>
    /// An integer representing the priority of the status. 
    /// Higher values indicate a more significant or final state in the business process lifecycle.
    /// </returns>
    /// <remarks>
    /// This rank is used to prevent "stale" or out-of-order messages from reverting 
    /// the job status to an earlier stage (e.g., preventing <c>Progress</c> from overwriting <c>Completed</c>).
    /// Default rank for final states (<c>Failed</c>, <c>Duplicate</c>, <c>Cancelled</c>, <c>TimedOut</c>) is <c>5</c>.
    /// </remarks>
    public static int GetStatusRank(TransferStatus status) => status switch
    {
        TransferStatus.None => 0,
        TransferStatus.Queued => 1,
        TransferStatus.Started => 2,
        TransferStatus.Progress => 3,
        TransferStatus.Completed => 4,
        _ => 5
    };
}
