using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainViewModel;

    public MainWindow(MainViewModel mainViewModel)
    {
        try
        {
            _mainViewModel = mainViewModel;
            DataContext = _mainViewModel;

            InitializeComponent();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
