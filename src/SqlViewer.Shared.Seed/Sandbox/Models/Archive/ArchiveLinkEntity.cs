using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("Archive_Links", Schema = "archive")]
public class ArchiveLinkEntity
{
    [Column("Source_ID")]
    public long SourceId { get; set; }

    [Column("Target_ID")]
    public Guid TargetId { get; set; }

    [Column("Link_Type")]
    public string? LinkType { get; set; }
}

