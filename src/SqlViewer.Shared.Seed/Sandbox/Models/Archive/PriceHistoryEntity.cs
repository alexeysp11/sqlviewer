using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("Price_History", Schema = "archive")]
public class PriceHistoryEntity
{
    [Key, Column("History_ID")]
    public long HistoryId { get; set; }

    [Column("Item_ID")]
    public int ItemId { get; set; }

    [Column("Changed_By_User")]
    public string? ChangedByUser { get; set; }

    [Column("Old_Value")]
    public decimal OldValue { get; set; }

    [Column("New_Value")]
    public decimal NewValue { get; set; }

    [Column("Valid_From")]
    public DateTime ValidFrom { get; set; }
}

