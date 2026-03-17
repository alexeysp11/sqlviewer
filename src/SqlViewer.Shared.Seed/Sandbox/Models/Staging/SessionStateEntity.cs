using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("sessionState", Schema = "staging")]
public class SessionStateEntity
{
    [Key, Column("Session_UID")]
    public Guid SessionUid { get; set; }

    [Column("Data_Blob")]
    [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
    public byte[]? DataBlob { get; set; }

    [Column("Is_Active")]
    public bool IsActive { get; set; }
}
