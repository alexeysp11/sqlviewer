using Riok.Mapperly.Abstractions;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Models;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.Security.Mappings;

[Mapper]
public partial class SeedMapper
{
    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.RefreshToken))]
    [MapperIgnoreTarget(nameof(User.RefreshTokenExpiry))]
    [MapValue(nameof(User.PasswordHash), "DEFAULT_SEED_HASH")]
    [MapProperty(nameof(UserSeedDto.Role), nameof(User.Role))]
    public partial User MapToUser(UserSeedDto dto);

#pragma warning disable IDE0051 // Remove unused private members
    private static SqlViewerAuthRole MapRole(string role) =>
        Enum.Parse<SqlViewerAuthRole>(role, ignoreCase: true);
#pragma warning restore IDE0051 // Remove unused private members
}
