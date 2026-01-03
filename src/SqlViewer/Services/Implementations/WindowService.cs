using System.Windows;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class WindowService : IWindowService
{
    public void ShowEtlWizard(
        IMetadataApiService metadataService,
        IQueryBuilderApiService queryBuilderService,
        string connectionString,
        VelocipedeDatabaseType databaseType)
    {
        EtlViewModel vm = new(metadataService, queryBuilderService)
        {
            SourceConnectionString = connectionString,
            SourceType = databaseType
        };
        EtlWindow win = new()
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        win.ShowDialog();
    }
}
