namespace SqlViewer.Shared.Dtos.Etl;

public class TransferStatusResponseDto
{
    public Guid CorrelationId { get; set; }
    public double Progress { get; set; }
    public string? StatusMessage { get; set; }
    public bool IsFinalState { get; set; }

    /// <summary>
    /// For filling ExecutionLogs.
    /// </summary>
    public List<string>? StepLogs { get; set; }
}