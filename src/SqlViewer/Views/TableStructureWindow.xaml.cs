using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

public partial class TableStructureWindow : Window
{
    private readonly TableStructureViewModel _etlViewModel;
    public TableStructureViewModel EtlViewModel => _etlViewModel;

    public TableStructureWindow(TableStructureViewModel etlViewModel)
    {
        _etlViewModel = etlViewModel;
        DataContext = _etlViewModel;

        InitializeComponent();
    }
}
