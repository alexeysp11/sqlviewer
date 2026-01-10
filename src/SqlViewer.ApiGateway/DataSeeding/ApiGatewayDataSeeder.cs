using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.ApiGateway.DbContexts;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Models;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.DataSeeding;

public class ApiGatewayDataSeeder(
    ApiGatewayDbContext context,
    IConfiguration config,
    IEncryptionService encryptionService,
    IPasswordHasher<User> passwordHasher) : IApiGatewayDataSeeder
{
    public async Task InitializeAsync()
    {
        string? encryptionKey = config["EncryptionSettings:Key"];
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new Exception("Encryption key is missing from configuration!");
        }

        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            // 1. Create admin.
            User admin = new()
            {
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword(null!, "admin")
            };
            context.Users.Add(admin);

            // 2. Create data source.
            string metadataConnectionString = config.GetConnectionString("MetadataConnection")
                ?? throw new Exception("Metadata connection string could not be null");
            DataSource dataSource = new()
            {
                Name = "pg-metadata-db",
                Description = "PostgreSQL database for metadata",
                EncryptedConnectionString = encryptionService.Encrypt(metadataConnectionString),
                Owner = admin,
                DbType = VelocipedeDatabaseType.PostgreSQL,
            };
            dataSource.Permissions = [new() { User = admin, DataSource = dataSource, AccessLevel = AccessLevelType.Admin }];
            context.DataSources.Add(dataSource);

            await context.SaveChangesAsync();
        }
    }
}
