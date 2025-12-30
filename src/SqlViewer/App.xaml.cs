using System.Windows;
using Microsoft.Extensions.Configuration;
using SqlViewer.Models;

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
}
