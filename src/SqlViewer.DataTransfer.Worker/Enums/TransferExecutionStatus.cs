namespace SqlViewer.DataTransfer.Worker.Enums;

/// <summary>
/// Defines the possible states of a data transfer execution lifecycle.
/// </summary>
public enum TransferExecutionStatus
{
    /// <summary>
    /// Default state, should not be used in active processes.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Initial setup and permission checks.
    /// </summary>
    Initializing = 1,
    
    /// <summary>
    /// Validating source and target database schemas.
    /// </summary>
    ValidatingSchema = 2,
    
    /// <summary>
    /// Active phase of copying data between databases.
    /// </summary>
    Transferring = 3,
    
    /// <summary>
    /// Finalization phase, resource cleanup.
    /// </summary>
    Finalizing = 4,
    
    /// <summary>
    /// Successfully completed.
    /// </summary>
    Completed = 5,
    
    /// <summary>
    /// Terminated due to an unrecoverable error.
    /// </summary>
    Failed = 6
}
