namespace SqlViewer.ApiGateway.Services;

public interface IAuthService
{
    Task<bool> VilidateByPasswordAsync(string username, string? password);
}
