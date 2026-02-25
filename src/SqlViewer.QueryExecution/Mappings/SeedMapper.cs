using Riok.Mapperly.Abstractions;
using SqlViewer.QueryExecution.Models;
using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.QueryExecution.Mappings;

[Mapper]
public partial class SeedMapper
{
    private const string DefaultConnectionString = SeedIdentifiers.DataSource.ConnectionString.Default;

    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.Permissions))]
    [MapperIgnoreTarget(nameof(User.OwnedSources))]
    [MapProperty(nameof(UserSeedDto.Role), nameof(User.Role))]
    public partial User MapToUser(UserSeedDto dto);

    [MapperIgnoreTarget(nameof(DataSource.Id))]
    [MapperIgnoreTarget(nameof(DataSource.Owner))]
    [MapperIgnoreTarget(nameof(DataSource.Permissions))]
    [MapperIgnoreSource(nameof(DataSourceSeedDto.OwnerUid))]
    [MapValue(nameof(DataSource.EncryptedConnectionString), DefaultConnectionString)]
    public partial DataSource MapToDataSource(DataSourceSeedDto dataSourceDto);
}
