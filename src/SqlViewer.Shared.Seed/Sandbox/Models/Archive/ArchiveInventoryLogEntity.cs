using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("inventory_logs", Schema = "archive")]
public class ArchiveInventoryLogEntity
{
    public long Id { get; set; }
    
    [Column("Product_UID")]
    public Guid ProductUid { get; set; }

    [Column("warehouseid")]
    public int WarehouseId { get; set; }

    [Column("quantitychange")]
    public int QuantityChange { get; set; }

    [Column("TimestampWithTZ")]
    public DateTime TimestampWithTz { get; set; }
}
