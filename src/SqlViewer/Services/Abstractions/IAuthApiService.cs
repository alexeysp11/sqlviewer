namespace SqlViewer.Services.Abstractions;

public interface IAuthApiService
{
    Task<bool> VerifyUserByPasswordAsync(string username, string password, CancellationToken ct = default);
    Task<bool> GuestLoginAsync(CancellationToken ct = default);
}
