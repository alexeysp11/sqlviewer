using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
