using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.UserControls;

namespace SqlViewer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainVM MainVM { get; set; }

    public MainWindow()
    {
        try
        {
            MainVM = new MainVM(this);

            MainVM.ConfigHelper.Initialize();

            MainVM.VisualVM.OpenView("LoginView");
            InitializeComponent();
            Hide();

            DataContext = MainVM;
            Menu.DataContext = MainVM;
            SqlPage.DataContext = MainVM;
            TablesPage.DataContext = MainVM;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
