using Riok.Mapperly.Abstractions;
using SqlViewer.Security.Data.Entities;
using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.Security.Mappings;

[Mapper]
public partial class SeedMapper
{
    private const string DefaultPassword = SeedIdentifiers.User.Password.Default;

    [MapperIgnoreTarget(nameof(UserEntity.Id))]
    [MapperIgnoreTarget(nameof(UserEntity.RefreshToken))]
    [MapperIgnoreTarget(nameof(UserEntity.RefreshTokenExpiry))]
    [MapValue(nameof(UserEntity.PasswordHash), DefaultPassword)]
    [MapProperty(nameof(UserSeedDto.Role), nameof(UserEntity.Role))]
    public partial UserEntity MapToUser(UserSeedDto dto);
}
