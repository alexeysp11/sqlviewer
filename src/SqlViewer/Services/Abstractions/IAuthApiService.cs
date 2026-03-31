namespace SqlViewer.Services.Abstractions;

public interface IAuthApiService
{
    Task<bool> VerifyUserByPasswordAsync(string username, string password);
    Task<bool> GuestLoginAsync();
}
