namespace SqlViewer.Common.Dtos.Etl;

public class TransferStatusResponseDto
{
    public Guid CorrelationId { get; set; }
    public double Progress { get; set; }
    public string? StatusMessage { get; set; }
    public bool IsFinalState { get; set; }
    public List<string>? StepLogs { get; set; } // For filling ExecutionLogs
}