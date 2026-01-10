using Microsoft.AspNetCore.DataProtection;

namespace SqlViewer.ApiGateway.Services.Implementations;

public class EncryptionService(IDataProtectionProvider provider) : IEncryptionService
{
    private readonly IDataProtector _protector = provider.CreateProtector("sqlviewer.v1");

    public string Encrypt(string plainText) => _protector.Protect(plainText);
    public string Decrypt(string cipherText) => _protector.Unprotect(cipherText);
}
