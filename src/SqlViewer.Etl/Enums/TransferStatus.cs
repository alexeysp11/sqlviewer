namespace SqlViewer.Etl.Enums;

/// <summary>
/// Represents the fine-grained execution status of a data transfer process.
/// Unlike <see cref="SagaStatus"/>, which tracks the high-level orchestration state, 
/// <see cref="TransferStatus"/> is used for logging historical events, reporting real-time progress 
/// to the UI, and performing post-transfer analytics.
/// </summary>
public enum TransferStatus
{
    /// <summary>No status has been reported yet.</summary>
    None = 0,
    /// <summary>The data transfer process has physically started in the worker service.</summary>
    Started = 1,
    /// <summary>Intermediary progress update containing metrics like rows processed.</summary>
    Progress = 2,
    /// <summary>The data transfer process finished successfully and all resources are released.</summary>
    Completed = 3,
    /// <summary>The data transfer process terminated prematurely due to a technical or business error.</summary>
    Failed = 4
}
