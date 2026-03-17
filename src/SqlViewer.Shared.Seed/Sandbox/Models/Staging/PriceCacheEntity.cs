using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("price_cache", Schema = "staging")]
public class PriceCacheEntity
{
    public required int Id { get; set; }

    [Column("product_sku")]
    public required string ProductSku { get; set; }

    [Column("old_price")]
    public decimal? OldPrice { get; set; }

    [Column("new_price")]
    public decimal? NewPrice { get; set; }
}
