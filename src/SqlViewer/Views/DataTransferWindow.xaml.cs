using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class DataTransferWindow : Window
{
    private readonly DataTransferViewModel _etlViewModel;
    public DataTransferViewModel EtlViewModel => _etlViewModel;

    public DataTransferWindow(DataTransferViewModel etlViewModel)
    {
        _etlViewModel = etlViewModel;
        DataContext = _etlViewModel;

        // Start polling when window is ready
        Loaded += (s, e) => _etlViewModel.OnViewLoaded();
        Closed += (s, e) => _etlViewModel.Dispose();

        InitializeComponent();
    }
}
