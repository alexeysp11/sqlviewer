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
}
