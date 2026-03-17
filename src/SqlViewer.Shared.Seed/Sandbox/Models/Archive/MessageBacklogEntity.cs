using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Archive;

[Table("message-backlog", Schema = "archive")]
public class MessageBacklogEntity
{
    [Key, Column("message-backlog-id")]
    public Guid Id { get; set; }

    [Column("sender-ref")]
    public string? SenderRef { get; set; }

    [Column("body-content")]
    public string? BodyContent { get; set; }

    [Column("archived-at")]
    public DateTime ArchivedAt { get; set; }
}

