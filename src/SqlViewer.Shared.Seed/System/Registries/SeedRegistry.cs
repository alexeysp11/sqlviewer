using SqlViewer.Shared.Seed.System.Constants;
using SqlViewer.Shared.Seed.System.Models;

namespace SqlViewer.Shared.Seed.System.Registries;

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
        ),
        new DataSourceSeedDto(
            Uid: SeedIdentifiers.DataSource.Uid.Security,
            Name: SeedIdentifiers.DataSource.Name.Security,
            Description: SeedIdentifiers.DataSource.Description.Security,
            DbType: SeedIdentifiers.DbType.Postgres,
            OwnerUid: SeedIdentifiers.User.Uid.Admin
        ),
        new DataSourceSeedDto(
            Uid: SeedIdentifiers.DataSource.Uid.QueryExecution,
            Name: SeedIdentifiers.DataSource.Name.QueryExecution,
            Description: SeedIdentifiers.DataSource.Description.QueryExecution,
            DbType: SeedIdentifiers.DbType.Postgres,
            OwnerUid: SeedIdentifiers.User.Uid.Admin
        ),
        new DataSourceSeedDto(
            Uid: SeedIdentifiers.DataSource.Uid.Sandbox,
            Name: SeedIdentifiers.DataSource.Name.Sandbox,
            Description: SeedIdentifiers.DataSource.Description.Sandbox,
            DbType: SeedIdentifiers.DbType.Postgres,
            OwnerUid: SeedIdentifiers.User.Uid.Admin
        )
    ];

    public static IEnumerable<DataSourcePermissionSeedDto> DataSourcePermissions =>
    [
        new DataSourcePermissionSeedDto(
            Uid: SeedIdentifiers.DataSourcePermission.Uid.AdminToMetadata,
            UserUid: SeedIdentifiers.User.Uid.Admin,
            DataSourceUid: SeedIdentifiers.DataSource.Uid.Metadata,
            AccessLevel: SeedIdentifiers.DataSourcePermission.AccessLevel.Admin
        ),
        new DataSourcePermissionSeedDto(
            Uid: SeedIdentifiers.DataSourcePermission.Uid.AdminToSecurity,
            UserUid: SeedIdentifiers.User.Uid.Admin,
            DataSourceUid: SeedIdentifiers.DataSource.Uid.Security,
            AccessLevel: SeedIdentifiers.DataSourcePermission.AccessLevel.Admin
        ),
        new DataSourcePermissionSeedDto(
            Uid: SeedIdentifiers.DataSourcePermission.Uid.AdminToQueryExecution,
            UserUid: SeedIdentifiers.User.Uid.Admin,
            DataSourceUid: SeedIdentifiers.DataSource.Uid.QueryExecution,
            AccessLevel: SeedIdentifiers.DataSourcePermission.AccessLevel.Admin
        ),
        new DataSourcePermissionSeedDto(
            Uid: SeedIdentifiers.DataSourcePermission.Uid.AdminToSandbox,
            UserUid: SeedIdentifiers.User.Uid.Admin,
            DataSourceUid: SeedIdentifiers.DataSource.Uid.Sandbox,
            AccessLevel: SeedIdentifiers.DataSourcePermission.AccessLevel.Admin
        )
    ];
}
