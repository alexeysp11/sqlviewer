using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.Etl.Core.Data.Entities;

public class TransferJobEntity
{
    public long Id { get; set; }
    public required Guid CorrelationId { get; set; }
    public required string Source { get; set; }
    public required string Target { get; set; }
    public TransferStatus CurrentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<TransferStatusLogEntity> Logs { get; set; } = [];
}
