using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("Yearly_Stats", Schema = "archive")]
public class YearlyStatsEntity
{
    [Key, Column("Year")]
    public int Year { get; set; }

    [Column("Total_Revenue")]
    public decimal TotalRevenue { get; set; }

    [Column("Total_Transactions")]
    public long TotalTransactions { get; set; }
}

