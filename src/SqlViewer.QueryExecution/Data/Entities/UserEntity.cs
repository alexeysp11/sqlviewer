using System.ComponentModel.DataAnnotations;
using SqlViewer.Common.Enums;

namespace SqlViewer.QueryExecution.Data.Entities;

public sealed record UserEntity
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }

    public required SqlViewerAuthRole Role { get; set; }

    public ICollection<DataSourcePermissionEntity> Permissions { get; set; } = [];
    public ICollection<DataSourceEntity> OwnedSources { get; set; } = [];
}
