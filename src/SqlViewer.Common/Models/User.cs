using System.ComponentModel.DataAnnotations;

namespace SqlViewer.Common.Models;

public sealed record User
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }

    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    public ICollection<DataSourcePermission> Permissions { get; set; } = [];
    public ICollection<DataSource> OwnedSources { get; set; } = [];
}
