using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Services;
using SqlViewer.QueryExecution.Data.DbContexts;
using SqlViewer.QueryExecution.Data.Entities;
using SqlViewer.QueryExecution.Enums;
using SqlViewer.QueryExecution.Mappings;
using SqlViewer.Shared.Seed.System.Constants;
using SqlViewer.Shared.Seed.System.Models;
using SqlViewer.Shared.Seed.System.Registries;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.QueryExecution.Data.DataSeeding;

public sealed class QueryExecutionDataSeeder(
    QueryExecutionDbContext context,
    IConfiguration config,
    SeedMapper seedMapper,
    IEncryptionService encryptionService) : IQueryExecutionDataSeeder
{
    private readonly Dictionary<Guid, DataSourceEntity> _createdDataSources = [];

    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        IEnumerable<UserEntity> users = SeedRegistry.Users.Select(seedMapper.MapToUser);
        foreach (UserEntity user in users)
        {
            await CreateUserIfNotExistsAsync(user);
            await CreateOwnedDataSources(user);
            await CreateDataSourcePermissions(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task CreateUserIfNotExistsAsync(UserEntity user)
    {
        long? existingUserId = await context.Users.Where(x => x.Username == user.Username).Select(x => (long?)x.Id).FirstOrDefaultAsync();
        if (!existingUserId.HasValue)
        {
            context.Users.Add(user);
        }
    }

    private async Task CreateOwnedDataSources(UserEntity user)
    {
        UserEntity existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username) ?? user;
        IEnumerable<DataSourceSeedDto> dataSourceDtos = SeedRegistry.DataSources.Where(ds => ds.OwnerUid == existingUser.Uid);
        foreach (DataSourceSeedDto dataSourceDto in dataSourceDtos)
        {
            long? dataSourceId = await context.DataSources
                .Where(ds => ds.Name == dataSourceDto.Name)
                .Select(ds => (long?)ds.Id)
                .FirstOrDefaultAsync();
            if (!dataSourceId.HasValue)
            {
                DataSourceEntity dataSource = seedMapper.MapToDataSource(dataSourceDto);
                dataSource.Owner = existingUser;
                string? connectionString = dataSourceDto.Name switch
                {
                    SeedIdentifiers.DataSource.Name.Metadata => config.GetConnectionString(ConnectionStrings.Metadata),
                    SeedIdentifiers.DataSource.Name.Security => config.GetConnectionString(ConnectionStrings.Security),
                    SeedIdentifiers.DataSource.Name.QueryExecution => config.GetConnectionString(ConnectionStrings.QueryExecution),
                    SeedIdentifiers.DataSource.Name.Sandbox => config.GetConnectionString(ConnectionStrings.Sandbox),
                    _ => dataSource.EncryptedConnectionString
                };
                if (!string.IsNullOrEmpty(connectionString))
                {
                    dataSource.EncryptedConnectionString = encryptionService.Encrypt(connectionString);
                }
                context.DataSources.Add(dataSource);
                _createdDataSources.Add(dataSource.Uid, dataSource);
            }
        }
    }

    private async Task CreateDataSourcePermissions(UserEntity user)
    {
        UserEntity existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username) ?? user;
        IEnumerable<DataSourcePermissionSeedDto> permissionDtos = SeedRegistry.DataSourcePermissions
            .Where(dsp => dsp.UserUid == existingUser.Uid);
        foreach (DataSourcePermissionSeedDto permissionDto in permissionDtos)
        {
            long? permissionId = await context.DataSourcePermissions
                .Where(dsp => dsp.DataSource.Uid == permissionDto.DataSourceUid)
                .Select(dsp => (long?)dsp.Id)
                .FirstOrDefaultAsync();
            if (!permissionId.HasValue)
            {
                DataSourceEntity dataSource = await context.DataSources.FirstOrDefaultAsync(ds => ds.Uid == permissionDto.DataSourceUid)
                    ?? _createdDataSources[permissionDto.DataSourceUid];
                DataSourcePermissionEntity permission = new()
                {
                    Uid = permissionDto.Uid,
                    User = existingUser,
                    DataSource = dataSource,
                    AccessLevel = Enum.Parse<AccessLevelType>(permissionDto.AccessLevel, false)
                };
                context.DataSourcePermissions.Add(permission);
            }
        }
    }
}
