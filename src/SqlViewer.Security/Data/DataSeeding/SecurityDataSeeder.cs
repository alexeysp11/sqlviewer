using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Models;
using SqlViewer.Shared.Seed.Models;
using SqlViewer.Shared.Seed.Registries;
using static SqlViewer.Common.Constants.ConfigurationKeys;

namespace SqlViewer.Security.Data.DataSeeding;

public sealed class SecurityDataSeeder(
    SecurityDbContext context,
    IConfiguration config,
    IPasswordHasher<User> passwordHasher) : ISecurityDataSeeder
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();
        await CreateAdminUserAsync();
        await context.SaveChangesAsync();
    }

    private async Task<User> CreateAdminUserAsync()
    {
        IEnumerable<UserSeedDto> users = SeedRegistry.Users;

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
}
