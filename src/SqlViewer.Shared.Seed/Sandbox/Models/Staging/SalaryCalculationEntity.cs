using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("salary_calculations", Schema = "staging")]
public class SalaryCalculationEntity
{
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column("Month")]
    public int Month { get; set; }

    [Column("Year")]
    public int Year { get; set; }

    [Column("BaseAmount")]
    public decimal BaseAmount { get; set; }

    [Column("Bonus_Pct")]
    public double BonusPct { get; set; }
}
