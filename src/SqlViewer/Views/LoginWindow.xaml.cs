using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _loginViewModel;
    public LoginViewModel LoginViewModel => _loginViewModel;

    public LoginWindow(LoginViewModel loginViewModel)
    {
        _loginViewModel = loginViewModel;
        DataContext = _loginViewModel;

        InitializeComponent();
    }
}
