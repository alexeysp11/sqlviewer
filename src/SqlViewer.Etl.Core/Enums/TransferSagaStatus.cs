namespace SqlViewer.Etl.Core.Enums;

/// <summary>
/// Represents the various states of the Data Transfer Saga execution.
/// </summary>
public enum TransferSagaStatus
{
    /// <summary>
    /// The saga has been initialized and the transfer command is received.
    /// </summary>
    Initial = 0,

    /// <summary>
    /// Checking connectivity and access rights for both source and target databases.
    /// </summary>
    AccessabilityCheck = 1,

    /// <summary>
    /// Validating schema compatibility and creating the necessary structures in the target database.
    /// </summary>
    SchemaValidation = 2,

    /// <summary>
    /// Data transmission is in progress (actual data movement or task delay).
    /// </summary>
    Transferring = 3,

    /// <summary>
    /// The data transfer has been successfully finished and verified.
    /// </summary>
    Completed = 4,

    /// <summary>
    /// The saga failed due to an error. Compensation logic (e.g., cleanup) may have been triggered.
    /// </summary>
    Failed = 5,

    /// <summary>
    /// The operation exceeded the maximum allowed execution time.
    /// </summary>
    TimedOut = 6,

    /// <summary>
    /// The transfer process was manually aborted by the user or the system.
    /// </summary>
    Cancelled = 7
}
