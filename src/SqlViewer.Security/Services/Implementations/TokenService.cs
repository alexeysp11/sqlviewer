using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using SqlViewer.Common.Enums;

namespace SqlViewer.Security.Services.Implementations;

public class TokenService(IConfiguration config) : ITokenService
{
    public string GenerateAccessToken(string username, SqlViewerAuthRole role)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Name, role is SqlViewerAuthRole.Guest ? role.ToString() : username),
            new(ClaimTypes.Role, role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

#nullable disable
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
#nullable restore
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        double expiryLifetimeMinutes = Convert.ToDouble(config["Jwt:ExpiryLifetimeMinutes"]);
        DateTime expires = DateTime.UtcNow.AddMinutes(expiryLifetimeMinutes);
        
        JwtSecurityToken token = new(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[64];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
