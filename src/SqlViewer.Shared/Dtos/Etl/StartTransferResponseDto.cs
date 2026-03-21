namespace SqlViewer.Shared.Dtos.Etl;

/// <summary>
/// Response from API Gateway upon successful initialization of the ETL process.
/// </summary>
public class StartTransferResponseDto
{
    /// <summary>
    /// Correlation identifier (SagaId).
    /// Used by the clients for subsequent status polling (Polling).
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// The time the request was created on the server side.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// A short message confirming that a task has been queued.
    /// </summary>
    public string Message { get; set; } = "Transfer process initialized and queued.";
}
