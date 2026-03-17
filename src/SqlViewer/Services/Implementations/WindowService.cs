using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SqlViewer.ViewModels;
using SqlViewer.Views;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class WindowService : IWindowService
{
    public void ShowEtlTableStructureWizard(
        string connectionString,
        VelocipedeDatabaseType databaseType)
    {
        EtlTableStructureWizardWindow etlWizardWindow = App.Services.GetRequiredService<EtlTableStructureWizardWindow>();
        etlWizardWindow.Owner = Application.Current.MainWindow;

        EtlTableTransferViewModel etlViewModel = etlWizardWindow.EtlViewModel;
        etlViewModel.SourceConnectionString = connectionString;
        etlViewModel.SourceType = databaseType;

        etlWizardWindow.ShowDialog();
    }

    public void ShowEtlDataTransferWindow(
        string connectionString,
        VelocipedeDatabaseType databaseType)
    {
        EtlDataTransferWindow etlWizardWindow = App.Services.GetRequiredService<EtlDataTransferWindow>();
        etlWizardWindow.Owner = Application.Current.MainWindow;

        EtlDataTransferViewModel etlViewModel = etlWizardWindow.EtlViewModel;
        etlViewModel.SourceConnectionString = connectionString;
        etlViewModel.SourceType = databaseType;

        etlWizardWindow.ShowDialog();
    }
}
