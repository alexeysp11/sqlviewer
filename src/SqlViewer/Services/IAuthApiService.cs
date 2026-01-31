
namespace SqlViewer.Services;

public interface IAuthApiService : IDisposable
{
    Task<bool> VerifyUserByPasswordAsync(string username, string password);
    Task<bool> GuestLoginAsync();
}
