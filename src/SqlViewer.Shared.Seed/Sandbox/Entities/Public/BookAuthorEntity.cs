using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Entities.Public;

[Table("BookAuthor", Schema = "public")]
public class BookAuthorEntity
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookID")]
    public int BookId { get; set; }

    [Column("AuthorID")]
    public int AuthorId { get; set; }
}
