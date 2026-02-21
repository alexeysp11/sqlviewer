using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Mappings;
using SqlViewer.Security.Models;
using SqlViewer.Shared.Seed.Registries;

namespace SqlViewer.Security.Data.DataSeeding;

public sealed class SecurityDataSeeder(
    SecurityDbContext context,
    IConfiguration config,
    SeedMapper seedMapper,
    IPasswordHasher<User> passwordHasher) : ISecurityDataSeeder
{
    public async Task InitializeAsync()
    {
        await context.Database.MigrateAsync();

        IEnumerable<User> userEntities = SeedRegistry.Users.Select(seedMapper.MapToUser);
        foreach (User userEntity in userEntities)
        {
            await CreateUserIfNotExistsAsync(userEntity);
        }
        await context.SaveChangesAsync();
    }

    private async Task CreateUserIfNotExistsAsync(User user)
    {
        User? existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
        if (existingUser is null)
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
            existingUser = new User
            {
                Username = user.Username,
                Uid = user.Uid,
                PasswordHash = passwordHasher.HashPassword(null!, password),
                Role = user.Role,
            };
            context.Users.Add(existingUser);
        }
    }
}
