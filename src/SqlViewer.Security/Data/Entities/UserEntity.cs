using System.ComponentModel.DataAnnotations;
using SqlViewer.Common.Enums;

namespace SqlViewer.Security.Data.Entities;

public sealed record UserEntity
{
    public long Id { get; set; }

    public Guid Uid { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }

    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    public required SqlViewerAuthRole Role { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public string? RefreshToken { get; set; }
}
