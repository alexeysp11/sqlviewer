using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class EtlWizardWindow : Window
{
    private readonly EtlViewModel _etlViewModel;
    public EtlViewModel EtlViewModel => _etlViewModel;

    public EtlWizardWindow(EtlViewModel etlViewModel)
    {
        _etlViewModel = etlViewModel;
        DataContext = _etlViewModel;

        InitializeComponent();
    }
}
