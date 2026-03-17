using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("Books", Schema = "public")]
public class BookEntity
{
    [Key, Column("Id")]
    public required int Id { get; set; }
    
    [Column("Title")]
    public required string Title { get; set; }

    [Column("ISBN-10")]
    public string? Isbn10 { get; set; }
    
    [Column("Year.Of.Publication")]
    public int YearOfPublication { get; set; }
}

