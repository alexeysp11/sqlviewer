using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("order_items", Schema = "public")]
public class OrderItemEntity
{
    [Column("OrderId")]
    public Guid OrderId { get; set; }

    [Column("ProductId")]
    public Guid ProductId { get; set; }
    
    [Column("Price")]
    public decimal Price { get; set; }
    
    [Column("Quantity")]
    public int Quantity { get; set; }
}

