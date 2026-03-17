using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("BookAuthor", Schema = "public")]
public class BookAuthorEntity
{
    [Column("BookID")]
    public int BookId { get; set; }

    [Column("AuthorID")]
    public int AuthorId { get; set; }
}

