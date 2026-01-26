using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Services;

namespace SqlViewer.ViewModels;

public partial class LoginViewModel(IAuthApiService authApiService) : ObservableObject, IDisposable
{
    public event Action<bool> LoginResultRequested;
    public event Action<string> ShowErrorRequested;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    private async Task LoginAsync(object parameter)
    {
        if (parameter is not PasswordBox passwordBox)
            return;

        IsBusy = true;
        try
        {
            string password = passwordBox.Password;
            bool isAuthenticated = await authApiService.VerifyUserByPasswordAsync(Username, password);
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
    private async Task GuestLoginAsync()
    {
        IsBusy = true;
        try
        {
            bool isAuthenticated = await authApiService.GuestLoginAsync();
            if (isAuthenticated)
            {
                LoginResultRequested?.Invoke(true);
            }
            else
            {
                ShowErrorRequested?.Invoke("Authentication failed");
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

    public void Dispose() => authApiService?.Dispose();
}
