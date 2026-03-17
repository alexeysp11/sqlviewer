using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("Tags_Internal", Schema = "staging")]
public class TagsInternalEntity
{
    [Key, Column("EntityID")]
    public Guid EntityId { get; set; }

    [Column("Tags_Array")]
    [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
    public string[]? TagsArray { get; set; }
}
