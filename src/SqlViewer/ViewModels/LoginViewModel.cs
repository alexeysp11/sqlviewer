using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SqlViewer.ViewModels;

public partial class LoginViewModel : ObservableObject
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

            bool isAuthenticated = await Task.Run(() => Username == "admin" && password == "admin");

            if (isAuthenticated)
            {
                LoginResultRequested?.Invoke(true);
            }
            else
            {
                ShowErrorRequested?.Invoke("Incorrect credentials");
            }
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
}
