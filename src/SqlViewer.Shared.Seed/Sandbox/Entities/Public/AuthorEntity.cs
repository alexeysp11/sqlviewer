using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Entities.Public;

[Table("authors", Schema = "public")]
public class AuthorEntity
{
    public required int Id { get; set; }

    [Column("full_name")]
    public required string FullName { get; set; }
}
