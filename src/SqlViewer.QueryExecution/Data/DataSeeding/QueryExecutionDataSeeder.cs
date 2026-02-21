using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Enums;
using SqlViewer.Common.Services;
using SqlViewer.QueryExecution.Data.DbContexts;
using SqlViewer.QueryExecution.Enums;
using SqlViewer.QueryExecution.Models;
using VelocipedeUtils.Shared.DbOperations.Enums;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.QueryExecution.Data.DataSeeding;

public sealed class QueryExecutionDataSeeder(
    QueryExecutionDbContext context,
    IConfiguration config,
    IEncryptionService encryptionService) : IQueryExecutionDataSeeder
{
    public async Task InitializeAsync()
    {
        string? encryptionKey = config[Encryption.Key];
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new Exception("Encryption key is missing from configuration");
        }

        await context.Database.MigrateAsync();

        User admin = await CreateAdminUserAsync();
        await CreateMetadataDataSource(admin);

        await context.SaveChangesAsync();
    }

    private async Task<User> CreateAdminUserAsync()
    {
        string? adminUsername = config[DefaultUserCredentials.Username.Admin];
        if (string.IsNullOrEmpty(adminUsername))
        {
            throw new Exception("Admin username is missing from configuration");
        }
        User? admin = await context.Users.FirstOrDefaultAsync(x => x.Username == adminUsername);
        if (admin is null)
        {
            admin = new()
            {
                Username = adminUsername,
                Role = SqlViewerAuthRole.Admin,
            };
            context.Users.Add(admin);
        }
        return admin;
    }

    private async Task CreateMetadataDataSource(User admin)
    {
        string? dataSourceName = config[DefaultDataSources.DbName.Metadata];
        if (string.IsNullOrEmpty(dataSourceName))
        {
            throw new Exception("Metadata datasource name is missing from configuration");
        }
        DataSource? dataSource = await context.DataSources.FirstOrDefaultAsync(x => x.Name == dataSourceName);
        if (dataSource is null)
        {
            string metadataConnectionString = config.GetConnectionString(ConnectionStrings.Metadata)
                ?? throw new Exception("Metadata connection string could not be null");
            string? dataSourceDescription = config[DefaultDataSources.DbDescription.Metadata];
            if (string.IsNullOrEmpty(dataSourceDescription))
            {
                throw new Exception("Metadata datasource description is missing from configuration");
            }
            dataSource = new()
            {
                Name = dataSourceName,
                Description = dataSourceDescription,
                EncryptedConnectionString = encryptionService.Encrypt(metadataConnectionString),
                Owner = admin,
                DbType = VelocipedeDatabaseType.PostgreSQL,
            };
            dataSource.Permissions = [new() { User = admin, DataSource = dataSource, AccessLevel = AccessLevelType.Admin }];
            context.DataSources.Add(dataSource);
        }
    }
}
