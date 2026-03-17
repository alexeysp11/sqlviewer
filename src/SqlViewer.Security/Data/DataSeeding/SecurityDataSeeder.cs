using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Data.Entities;
using SqlViewer.Security.Mappings;
using SqlViewer.Shared.Seed.System.Registries;

namespace SqlViewer.Security.Data.DataSeeding;

public sealed class SecurityDataSeeder(
    SecurityDbContext context,
    IConfiguration config,
    SeedMapper seedMapper,
    IPasswordHasher<UserEntity> passwordHasher) : ISecurityDataSeeder
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        IEnumerable<UserEntity> users = SeedRegistry.Users.Select(seedMapper.MapToUser);
        foreach (UserEntity user in users)
        {
            await CreateUserIfNotExistsAsync(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task CreateUserIfNotExistsAsync(UserEntity user)
    {
        long? existingUserId = await context.Users.Where(x => x.Username == user.Username).Select(x => (long?)x.Id).FirstOrDefaultAsync();
        if (!existingUserId.HasValue)
        {
            string? password = user.Role switch
            {
                SqlViewerAuthRole.Admin => config[ConfigurationKeys.DefaultUserCredentials.Password.Admin],
                SqlViewerAuthRole.Guest => config[ConfigurationKeys.DefaultUserCredentials.Password.Guest],
                _ => user.PasswordHash
            };
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception($"Password is missing from configuration for the specified user: {user.Username}");
            }
            user.PasswordHash = passwordHasher.HashPassword(null!, password);
            context.Users.Add(user);
        }
    }
}
