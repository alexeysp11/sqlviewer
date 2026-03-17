using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Public;

[Table("Categories", Schema = "public")]
public class CategoryEntity
{
    [Key, Column("CategoryID")]
    public required int CategoryId { get; set; }

    [Column("categoryname")]
    public required string CategoryName { get; set; }

    [Column("category-code")]
    public required string CategoryCode { get; set; }
}

