namespace SqlViewer.Security.Models;

public sealed record QueryLog
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public long UserId { get; set; }
    public long DataSourceId { get; set; }
    public required string QueryText { get; set; }
    public long ExecutionTimeMs { get; set; }
}
