using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("SalesOrders", Schema = "public")]
public class SalesOrderEntity
{
    [Key, Column("OrderId")]
    public Guid OrderId { get; set; }
    
    [Column("order date")]
    public DateTime OrderDate { get; set; }

    [Column("Customer ID")]
    public int CustomerId { get; set; }

    [Column("Total Amount: Expected")]
    public decimal TotalAmountExpected { get; set; }
    
    [Column("Total Amount: Factual")]
    public decimal TotalAmountFactual { get; set; }
    
    [Column("status")]
    public string? Status { get; set; }
}

