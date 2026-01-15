using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.ApiGateway.DbContexts;
using SqlViewer.Common.Models;

namespace SqlViewer.ApiGateway.Services.Implementations;

public sealed class AuthService(ApiGatewayDbContext dbContext, IPasswordHasher<User> passwordHasher) : IAuthService
{
    public async Task<bool> VilidateByPasswordAsync(string username, string? password)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
            return false;

        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
            user: null!,
            hashedPassword: user.PasswordHash,
            providedPassword: password!);

        return result is not PasswordVerificationResult.Failed;
    }
}
