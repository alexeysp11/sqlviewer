using Riok.Mapperly.Abstractions;
using SqlViewer.QueryExecution.Data.Entities;
using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;

namespace SqlViewer.QueryExecution.Mappings;

[Mapper]
public partial class SeedMapper
{
    private const string DefaultConnectionString = SeedIdentifiers.DataSource.ConnectionString.Default;

    [MapperIgnoreTarget(nameof(UserEntity.Id))]
    [MapperIgnoreTarget(nameof(UserEntity.Permissions))]
    [MapperIgnoreTarget(nameof(UserEntity.OwnedSources))]
    [MapProperty(nameof(UserSeedDto.Role), nameof(UserEntity.Role))]
    public partial UserEntity MapToUser(UserSeedDto dto);

    [MapperIgnoreTarget(nameof(DataSourceEntity.Id))]
    [MapperIgnoreTarget(nameof(DataSourceEntity.Owner))]
    [MapperIgnoreTarget(nameof(DataSourceEntity.Permissions))]
    [MapperIgnoreSource(nameof(DataSourceSeedDto.OwnerUid))]
    [MapValue(nameof(DataSourceEntity.EncryptedConnectionString), DefaultConnectionString)]
    public partial DataSourceEntity MapToDataSource(DataSourceSeedDto dataSourceDto);
}
