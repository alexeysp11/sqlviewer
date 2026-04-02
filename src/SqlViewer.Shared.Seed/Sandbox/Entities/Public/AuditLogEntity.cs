using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Entities.Public;

[Table("audit_logs", Schema = "public")]
public class AuditLogEntity
{
    public long Id { get; set; }

    [Column("User_Data", TypeName = "jsonb")]
    public string? UserData { get; set; }

    [Column("ActionDetails")]
    public string? ActionDetails { get; set; }

    [Column("executed_at")]
    public DateTime ExecutedAt { get; set; }
}
