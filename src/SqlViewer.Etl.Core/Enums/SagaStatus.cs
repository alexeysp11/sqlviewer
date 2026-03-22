namespace SqlViewer.Etl.Core.Enums;

/// <summary>
/// Defines the possible states of the Data Transfer Saga lifecycle.
/// </summary>
public enum SagaStatus
{
    /// <summary>
    /// Saga has been created but no commands have been sent yet.
    /// </summary>
    Initial = 0,

    /// <summary>
    /// The transfer command has been published to Kafka, waiting for worker to start.
    /// </summary>
    CommandSent = 1,

    /// <summary>
    /// Worker has confirmed start and is currently transferring data.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Transfer finished successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Transfer failed or was aborted due to error.
    /// </summary>
    Faulted = 4,

    /// <summary>
    /// Saga exceeded its allowed time and was marked as failed by timeout monitor.
    /// </summary>
    TimedOut = 5
}
