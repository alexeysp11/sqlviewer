using System.ComponentModel.DataAnnotations;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Models;

public sealed record DataSource
{
    public long Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public VelocipedeDatabaseType DbType { get; set; }
    
    [MaxLength(2000)]
    public required string EncryptedConnectionString { get; set; }

    public long OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public ICollection<DataSourcePermission> Permissions { get; set; } = [];
}
