using System.Windows;
using Microsoft.Extensions.Configuration;
using SqlViewer.Models;
using SqlViewer.ViewModels;
using SqlViewer.Views;

namespace SqlViewer;

public partial class App : Application
{
    public static AppSettings AppSettings { get; private set; }

    public App()
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json").Build();
        AppSettings = configuration.GetSection("AppSettings").Get<AppSettings>()
            ?? throw new Exception($"Could not initialize {nameof(AppSettings)}");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        LoginViewModel loginVM = new();
        LoginWindow loginWindow = new() { DataContext = loginVM };

        loginVM.LoginResultRequested += (isSuccess) =>
        {
            if (isSuccess)
            {
                MainWindow mainWindow = new();
                mainWindow.Show();

                MainWindow = mainWindow;
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                
                loginWindow.Close();
            }
            else
            {
                Shutdown();
            }
        };
        loginVM.ShowErrorRequested += (errorMessage) =>
        {
            MessageBox.Show(errorMessage, "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
        };
        loginWindow.Closed += (s, ev) =>
        {
            if (MainWindow == null || !MainWindow.IsVisible)
            {
                Shutdown();
            }
        };

        loginWindow.Show();
    }
}
