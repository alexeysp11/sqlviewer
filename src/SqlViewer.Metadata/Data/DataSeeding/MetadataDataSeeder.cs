using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Services;
using SqlViewer.Metadata.Data.DbContexts;
using SqlViewer.Metadata.Mappings;
using SqlViewer.Metadata.Models;
using SqlViewer.Shared.Seed.Constants;
using SqlViewer.Shared.Seed.Models;
using SqlViewer.Shared.Seed.Registries;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.Metadata.Data.DataSeeding;

public sealed class MetadataDataSeeder(
    MetadataDbContext context,
    IConfiguration config,
    SeedMapper seedMapper,
    IEncryptionService encryptionService) : IMetadataDataSeeder
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        IEnumerable<User> users = SeedRegistry.Users.Select(seedMapper.MapToUser);
        foreach (User user in users)
        {
            await CreateUserIfNotExistsAsync(user);
            await CreateOwnedDataSources(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task CreateUserIfNotExistsAsync(User user)
    {
        long? existingUserId = await context.Users.Where(x => x.Username == user.Username).Select(x => (long?)x.Id).FirstOrDefaultAsync();
        if (!existingUserId.HasValue)
        {
            context.Users.Add(user);
        }
    }

    private async Task CreateOwnedDataSources(User user)
    {
        User existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username) ?? user;
        IEnumerable<DataSourceSeedDto> dataSourceDtos = SeedRegistry.DataSources.Where(ds => ds.OwnerUid == existingUser.Uid);
        foreach (DataSourceSeedDto dataSourceDto in dataSourceDtos)
        {
            long? dataSourceId = await context.DataSources
                .Where(ds => ds.Name == dataSourceDto.Name)
                .Select(ds => (long?)ds.Id)
                .FirstOrDefaultAsync();
            if (!dataSourceId.HasValue)
            {
                DataSource dataSource = seedMapper.MapToDataSource(dataSourceDto);
                dataSource.Owner = existingUser;
                string? connectionString = dataSourceDto.Name switch
                {
                    SeedIdentifiers.DataSource.Name.Metadata => config.GetConnectionString(ConnectionStrings.Metadata),
                    SeedIdentifiers.DataSource.Name.Security => config.GetConnectionString(ConnectionStrings.Security),
                    SeedIdentifiers.DataSource.Name.QueryExecution => config.GetConnectionString(ConnectionStrings.QueryExecution),
                    _ => dataSource.EncryptedConnectionString
                };
                if (!string.IsNullOrEmpty(connectionString))
                {
                    dataSource.EncryptedConnectionString = encryptionService.Encrypt(connectionString);
                }
                context.DataSources.Add(dataSource);
            }
        }
    }
}
