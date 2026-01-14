using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.ApiHandlers;
using SqlViewer.Services.Implementations;

namespace SqlViewer.ViewModels;

public partial class LoginViewModel : ObservableObject, IDisposable
{
    public event Action<bool> LoginResultRequested;
    public event Action<string> ShowErrorRequested;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    private readonly AuthApiService _authApiService;

    public LoginViewModel()
    {
        HttpHandler httpHandler = new();
        _authApiService = new AuthApiService(httpHandler);
    }

    [RelayCommand]
    private async Task LoginAsync(object parameter)
    {
        if (parameter is not PasswordBox passwordBox)
            return;

        IsBusy = true;
        try
        {
            string password = passwordBox.Password;
            bool isAuthenticated = await _authApiService.VerifyUserByPasswordAsync(Username, password);
            if (isAuthenticated)
            {
                LoginResultRequested?.Invoke(true);
            }
            else
            {
                ShowErrorRequested?.Invoke("Incorrect credentials");
            }
        }
        catch (Exception ex)
        {
            ShowErrorRequested?.Invoke(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GuestLogin()
    {
        LoginResultRequested?.Invoke(true);
    }

    public void Dispose() => _authApiService?.Dispose();
}
