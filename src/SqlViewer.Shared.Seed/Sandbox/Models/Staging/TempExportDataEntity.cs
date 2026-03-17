using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlViewer.Shared.Seed.Sandbox.Models.Staging;

[Table("Temp Export Data", Schema = "staging")]
public class TempExportDataEntity
{
    [Key, Column("Row_ID")]
    public int RowId { get; set; }

    [Column("Raw_Payload")]
    public string? RawPayload { get; set; }

    [Column("Imported_At")]
    public DateTime ImportedAt { get; set; }
}
