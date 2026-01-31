using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.ApiGateway.Data.DbContexts;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Enums;
using SqlViewer.Common.Models;
using VelocipedeUtils.Shared.DbOperations.Enums;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.ApiGateway.Data.DataSeeding;

public sealed class ApiGatewayDataSeeder(
    ApiGatewayDbContext context,
    IConfiguration config,
    IEncryptionService encryptionService,
    IPasswordHasher<User> passwordHasher) : IApiGatewayDataSeeder
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
        string? adminUsername = config[DefaultUserCredentials.AdminUsername];
        if (string.IsNullOrEmpty(adminUsername))
        {
            throw new Exception("Admin username is missing from configuration");
        }
        User? admin = await context.Users.FirstOrDefaultAsync(x => x.Username == adminUsername);
        if (admin is null)
        {
            string? adminPassword = config[DefaultUserCredentials.AdminPassword];
            if (string.IsNullOrEmpty(adminPassword))
            {
                throw new Exception("Admin password is missing from configuration");
            }
            admin = new()
            {
                Username = adminUsername,
                PasswordHash = passwordHasher.HashPassword(null!, adminPassword),
                Role = SqlViewerAuthRole.Admin,
            };
            context.Users.Add(admin);
        }
        return admin;
    }

    private async Task CreateMetadataDataSource(User admin)
    {
        string? dataSourceName = config[DefaultDataSources.MetadataDbName];
        if (string.IsNullOrEmpty(dataSourceName))
        {
            throw new Exception("Metadata datasource name is missing from configuration");
        }
        DataSource? dataSource = await context.DataSources.FirstOrDefaultAsync(x => x.Name == dataSourceName);
        if (dataSource is null)
        {
            string metadataConnectionString = config.GetConnectionString(ConnectionStrings.Metadata)
                ?? throw new Exception("Metadata connection string could not be null");
            string? dataSourceDescription = config[DefaultDataSources.MetadataDbDescription];
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
