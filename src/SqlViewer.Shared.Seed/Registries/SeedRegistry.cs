using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.Shared.Seed.Registries;

public static class SeedRegistry
{
    public static IEnumerable<UserSeedDto> Users =>
    [
        new UserSeedDto(
            Uid: SeedIdentifiers.User.Uid.Admin,
            Username: "admin",
            Role: SeedIdentifiers.Roles.Admin
        )
    ];

    public static IEnumerable<DataSourceSeedDto> DataSources =>
    [
        new DataSourceSeedDto(
            Uid: SeedIdentifiers.DataSource.Uid.Metadata,
            Name: "Metadata Source",
            Description: "System Database",
            DbType: "PostgreSQL",
            OwnerUid: SeedIdentifiers.User.Uid.Admin
        )
    ];
}
