using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("Deleted_Users_Registry", Schema = "archive")]
public class DeletedUserRegistryEntity
{
    [Key, Column("user id")]
    public int UserId { get; set; }

    [Column("profile snapshot", TypeName = "jsonb")]
    public string? ProfileSnapshot { get; set; }

    [Column("deletion reason")]
    public string? DeletionReason { get; set; }

    [Column("deleted at")]
    public DateOnly? DeletedAt { get; set; }
}

