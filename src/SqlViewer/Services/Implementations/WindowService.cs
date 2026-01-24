using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class WindowService : IWindowService
{
    public void ShowEtlWizard(
        ISqlApiService sqlApiService,
        IMetadataApiService metadataService,
        IQueryBuilderApiService queryBuilderService,
        string connectionString,
        VelocipedeDatabaseType databaseType)
    {
        EtlWizardWindow etlWizardWindow = App.Services.GetRequiredService<EtlWizardWindow>();
        etlWizardWindow.Owner = Application.Current.MainWindow;

        EtlViewModel etlViewModel = etlWizardWindow.EtlViewModel;
        etlViewModel.SourceConnectionString = connectionString;
        etlViewModel.SourceType = databaseType;

        etlWizardWindow.ShowDialog();
    }
}
