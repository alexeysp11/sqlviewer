using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Entities.Public;

[Table("regions", Schema = "public")]
public class RegionEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Timezone { get; set; }

    [Column("timezone_offset")]
    public int TimezoneOffset { get; set; }
}
