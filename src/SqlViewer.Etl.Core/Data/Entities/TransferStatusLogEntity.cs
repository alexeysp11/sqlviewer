using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.Etl.Core.Data.Entities;

public class TransferStatusLogEntity
{
    public long Id { get; set; }
    public Guid JobId { get; set; }
    public TransferStatus Status { get; set; }
    public string? MetadataJson { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
