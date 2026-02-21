using Riok.Mapperly.Abstractions;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Models;
using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.Security.Mappings;

[Mapper]
public partial class SeedMapper
{
    private const string DefaultPassword = SeedIdentifiers.User.Password.Default;

    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.RefreshToken))]
    [MapperIgnoreTarget(nameof(User.RefreshTokenExpiry))]
    [MapValue(nameof(User.PasswordHash), DefaultPassword)]
    [MapProperty(nameof(UserSeedDto.Role), nameof(User.Role), Use = nameof(MapRole))]
    public partial User MapToUser(UserSeedDto dto);

#pragma warning disable IDE0051 // Remove unused private members
    private static SqlViewerAuthRole MapRole(string role) =>
        Enum.Parse<SqlViewerAuthRole>(role, ignoreCase: true);
#pragma warning restore IDE0051 // Remove unused private members
}
