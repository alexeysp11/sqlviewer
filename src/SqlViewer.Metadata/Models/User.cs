using System.ComponentModel.DataAnnotations;
using SqlViewer.Common.Enums;

namespace SqlViewer.Metadata.Models;

public sealed record User
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }

    public required SqlViewerAuthRole Role { get; set; }

    public ICollection<DataSourcePermission> Permissions { get; set; } = [];
    public ICollection<DataSource> OwnedSources { get; set; } = [];
}
