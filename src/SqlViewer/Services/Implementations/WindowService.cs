using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SqlViewer.Services.Abstractions;
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
        TableStructureWindow etlWizardWindow = App.Services.GetRequiredService<TableStructureWindow>();
        etlWizardWindow.Owner = Application.Current.MainWindow;

        TableStructureViewModel etlViewModel = etlWizardWindow.EtlViewModel;
        etlViewModel.SourceConnectionString = connectionString;
        etlViewModel.SourceType = databaseType;

        etlWizardWindow.ShowDialog();
    }

    public void ShowEtlDataTransferWindow(
        string connectionString,
        VelocipedeDatabaseType databaseType)
    {
        DataTransferWindow etlWizardWindow = App.Services.GetRequiredService<DataTransferWindow>();
        etlWizardWindow.Owner = Application.Current.MainWindow;

        DataTransferViewModel etlViewModel = etlWizardWindow.EtlViewModel;
        etlViewModel.SourceConnectionString = connectionString;
        etlViewModel.SourceType = databaseType;

        etlWizardWindow.ShowDialog();
    }
}
