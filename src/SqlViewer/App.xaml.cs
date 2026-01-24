using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlViewer.ApiHandlers;
using SqlViewer.ApiHandlers.Implementations;
using SqlViewer.Models;
using SqlViewer.Services;
using SqlViewer.Services.Implementations;
using SqlViewer.ViewModels;
using SqlViewer.Views;

namespace SqlViewer;

public partial class App : Application
{
    public static AppSettings AppSettings { get; private set; }
    public static IServiceProvider Services { get; private set; }

    public App()
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json").Build();
        AppSettings = configuration.GetSection("AppSettings").Get<AppSettings>()
            ?? throw new Exception($"Could not initialize {nameof(AppSettings)}");

        Services = ConfigureServices();
    }

    private static ServiceProvider ConfigureServices()
    {
        ServiceCollection services = new();

        // 1. Services.
        services.AddSingleton<IAuthApiService, AuthApiService>();
        services.AddSingleton<IDocsApiService, DocsApiService>();
        services.AddSingleton<IMetadataApiService, MetadataApiService>();
        services.AddSingleton<IQueryBuilderApiService, QueryBuilderApiService>();
        services.AddSingleton<ISqlApiService, SqlApiService>();
        services.AddSingleton<IWindowService, WindowService>();

        // 2. HTTP handlers.
        services.AddSingleton<IAuthHttpHandler, AuthHttpHandler>();
        services.AddSingleton<IDocsHttpHandler, DocsHttpHandler>();
        services.AddSingleton<IMetadataHttpHandler, MetadataHttpHandler>();
        services.AddSingleton<IQueryBuilderHttpHandler, QueryBuilderHttpHandler>();
        services.AddSingleton<ISqlHttpHandler, SqlHttpHandler>();

        // Register HttpClient with base address.
        //services.AddHttpClient("MyApi", client => {
        //    client.BaseAddress = new Uri("https://api.example.com/");
        //});

        // 3. ViewModels.
        services.AddTransient<LoginViewModel>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<EtlViewModel>();

        // 4. Views.
        services.AddTransient<LoginWindow>();
        services.AddSingleton<MainWindow>();
        services.AddTransient<EtlWizardWindow>();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        LoginWindow loginWindow = Services.GetRequiredService<LoginWindow>();
        LoginViewModel loginViewModel = loginWindow.LoginViewModel;
        loginWindow.Show();

        loginViewModel.LoginResultRequested += (isSuccess) =>
        {
            if (isSuccess)
            {
                MainWindow mainWindow = Services.GetRequiredService<MainWindow>();
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
        loginViewModel.ShowErrorRequested += (errorMessage) =>
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
