using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Models;

namespace SqlViewer.Security.Services.Implementations;

public sealed class AuthService(
    SecurityDbContext dbContext,
    IConfiguration config,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<bool> VilidateByPasswordAsync(string username, string? password)
    {
        if (string.IsNullOrEmpty(username))
            throw new InvalidOperationException($"Parameter {nameof(username)} should be specified");

        User? user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
            return false;

        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
            user: null!,
            hashedPassword: user.PasswordHash,
            providedPassword: password!);

        return result is not PasswordVerificationResult.Failed;
    }

    public async Task<LoginResponseDto> CreateSessionAsync(string username)
    {
        User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username)
            ?? throw new InvalidOperationException("User not found");

        string accessToken = tokenService.GenerateAccessToken(user.Username, user.Role);
        string refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync();

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Role = user.Role,
            ExpiresInSeconds = Convert.ToDouble(config["Jwt:ExpiryLifetimeMinutes"]) * 60
        };
    }

    public async Task<LoginResponseDto> RefreshSessionAsync(string refreshToken)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Session expired. Please, sign in again");
        }
        return await CreateSessionAsync(user.Username);
    }

    public LoginResponseDto CreateGuestSession()
    {
        string accessToken = tokenService.GenerateAccessToken(username: null!, role: SqlViewerAuthRole.Guest);
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            Role = SqlViewerAuthRole.Guest,
            ExpiresInSeconds = Convert.ToDouble(config["Jwt:ExpiryLifetimeMinutes"]) * 60
        };
    }
}
