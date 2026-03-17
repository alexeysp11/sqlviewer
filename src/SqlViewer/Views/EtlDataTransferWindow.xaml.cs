using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class EtlDataTransferWindow : Window
{
    private readonly EtlDataTransferViewModel _etlViewModel;
    public EtlDataTransferViewModel EtlViewModel => _etlViewModel;

    public EtlDataTransferWindow(EtlDataTransferViewModel etlViewModel)
    {
        _etlViewModel = etlViewModel;
        DataContext = _etlViewModel;

        InitializeComponent();
    }
}
