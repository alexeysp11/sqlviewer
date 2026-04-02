using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Entities.Archive;

[Table("Archive_Links", Schema = "archive")]
public class ArchiveLinkEntity
{
    [Column("Source_ID")]
    public required long SourceId { get; set; }

    [Column("Target_ID")]
    public required Guid TargetId { get; set; }

    [Column("Link_Type")]
    public string? LinkType { get; set; }
}
