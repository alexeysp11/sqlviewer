using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("Code_Internal", Schema = "staging")]
public class StagingCodeInternalEntity
{
    public required int Id { get; set; }

    [Column("internal_value")]
    public required string InternalValue { get; set; }
}
