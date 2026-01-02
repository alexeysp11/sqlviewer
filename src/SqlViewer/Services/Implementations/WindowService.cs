using System.Windows;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class WindowService(IMetadataApiService metadataService) : IWindowService
{
    private readonly IMetadataApiService _metadataService = metadataService;

    public void ShowEtlWizard(string connectionString, VelocipedeDatabaseType databaseType)
    {
        EtlViewModel vm = new(_metadataService);
        EtlWindow win = new()
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        win.ShowDialog();
    }
}
