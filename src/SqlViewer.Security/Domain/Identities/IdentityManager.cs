using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Auth;
using SqlViewer.Shared.Enums;
using SqlViewer.Security.Data.DbContexts;
using SqlViewer.Security.Data.Entities;
using SqlViewer.Security.Domain.Tokens;

namespace SqlViewer.Security.Domain.Identities;

public sealed class IdentityManager(
    SecurityDbContext dbContext,
    IConfiguration config,
    IPasswordHasher<UserEntity> passwordHasher,
    ITokenProvider tokenService) : IIdentityManager
{
    public async Task<bool> VilidateByPasswordAsync(string username, string? password)
    {
        if (string.IsNullOrEmpty(username))
            throw new InvalidOperationException($"Parameter {nameof(username)} should be specified");

        UserEntity? user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
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
        UserEntity user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username)
            ?? throw new InvalidOperationException("UserEntity not found");

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
            UserUid = user.Uid,
            Role = user.Role,
            ExpiresInSeconds = Convert.ToDouble(config[ConfigurationKeys.Jwt.ExpiryLifetimeMinutes]) * 60
        };
    }

    public async Task<LoginResponseDto> RefreshSessionAsync(string refreshToken)
    {
        UserEntity? user = await dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
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
            ExpiresInSeconds = Convert.ToDouble(config[ConfigurationKeys.Jwt.ExpiryLifetimeMinutes]) * 60
        };
    }
}
