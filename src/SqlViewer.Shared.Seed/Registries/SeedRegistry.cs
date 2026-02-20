using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.Shared.Seed.Registries;

public static class SeedRegistry
{
    public static IEnumerable<UserSeedDto> Users =>
    [
        new UserSeedDto(
            Uid: SeedIdentifiers.User.Uid.Admin,
            Username: SeedIdentifiers.User.Username.Admin,
            Role: SeedIdentifiers.Roles.Admin
        ),
        new UserSeedDto(
            Uid: SeedIdentifiers.User.Uid.Guest,
            Username: SeedIdentifiers.User.Username.Guest,
            Role: SeedIdentifiers.Roles.Guest
        )
    ];

    public static IEnumerable<DataSourceSeedDto> DataSources =>
    [
        new DataSourceSeedDto(
            Uid: SeedIdentifiers.DataSource.Uid.Metadata,
            Name: SeedIdentifiers.DataSource.Name.Metadata,
            Description: SeedIdentifiers.DataSource.Description.Metadata,
            DbType: SeedIdentifiers.DbType.Postgres,
            OwnerUid: SeedIdentifiers.User.Uid.Admin
        )
    ];

    public static IEnumerable<DataSourcePermissionSeedDto> DataSourcePermissions =>
    [
        new DataSourcePermissionSeedDto(
            UserUid: SeedIdentifiers.User.Uid.Admin,
            DataSourceUid: SeedIdentifiers.DataSource.Uid.Metadata,
            AccessLevel: SeedIdentifiers.DataSourcePermission.AccessLevel.Admin
        )
    ];
}
