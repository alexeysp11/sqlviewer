using System.IO;
using System.Security.Cryptography;
using System.Text;
using SqlViewer.Common.Dtos.Auth;

namespace SqlViewer.StorageContexts.Implementations;

#nullable enable

public sealed record UserContext : IUserContext
{
    public LoginResponseDto? CurrentUser { get; set; }

    public static void SaveToken(string token)
    {
        byte[] data = Encoding.UTF8.GetBytes(token);
        byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes("token.dat", encrypted);
    }

    public static string? LoadToken()
    {
        if (!File.Exists("token.dat"))
            return null;

        byte[] encrypted = File.ReadAllBytes("token.dat");
        byte[] data = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(data);
    }
}
