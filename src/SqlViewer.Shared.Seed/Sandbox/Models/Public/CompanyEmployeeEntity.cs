using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("companyEmployee", Schema = "public")]
public class CompanyEmployeeEntity
{
    public int Id { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [Column("employee_id")]
    public int EmployeeId { get; set; }
}

