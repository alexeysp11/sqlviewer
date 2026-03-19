using SqlViewer.Etl.Enums;

namespace SqlViewer.Etl.Data.Entities;

public class TransferStatusLogEntity
{
    public long Id { get; set; }
    public Guid JobId { get; set; }
    public TransferStatus Status { get; set; }
    public string? MetadataJson { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
