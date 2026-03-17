using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("returns", Schema = "public")]
public class ReturnEntity
{
    [Key, Column("ID")]
    public Guid Id { get; set; }

    [Column("OrderId")]
    public Guid OrderId { get; set; }

    [Column("Customer_Id")]
    public int CustomerId { get; set; }

    [Column("totalAmount")]
    public decimal TotalAmount { get; set; }

    [Column("status")]
    public string? Status { get; set; }
}

