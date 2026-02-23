using Riok.Mapperly.Abstractions;
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
    [MapProperty(nameof(UserSeedDto.Role), nameof(User.Role))]
    public partial User MapToUser(UserSeedDto dto);
}
