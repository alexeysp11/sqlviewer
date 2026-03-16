using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class EtlTableStructureWizardWindow : Window
{
    private readonly EtlTableTransferViewModel _etlViewModel;
    public EtlTableTransferViewModel EtlViewModel => _etlViewModel;

    public EtlTableStructureWizardWindow(EtlTableTransferViewModel etlViewModel)
    {
        _etlViewModel = etlViewModel;
        DataContext = _etlViewModel;

        InitializeComponent();
    }
}
