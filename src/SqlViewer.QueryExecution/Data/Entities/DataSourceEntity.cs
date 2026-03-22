using System.ComponentModel.DataAnnotations;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.QueryExecution.Data.Entities;

public sealed record DataSourceEntity
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public VelocipedeDatabaseType DbType { get; set; }

    [MaxLength(2000)]
    public required string EncryptedConnectionString { get; set; }

    public UserEntity Owner { get; set; } = null!;

    public ICollection<DataSourcePermissionEntity> Permissions { get; set; } = [];
}
