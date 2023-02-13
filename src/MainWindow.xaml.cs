using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.UserControls; 

namespace SqlViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main ViewModel 
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of MainWindow
        /// </summary>
        public MainWindow()
        {
            try
            {
                this.MainVM = new MainVM(this); 

                this.MainVM.VisualVM.OpenView("LoginView"); 
                InitializeComponent();
                this.Hide();

                this.DataContext = this.MainVM;
                this.Menu.DataContext = this.MainVM; 
                this.SqlPage.DataContext = this.MainVM; 
                this.TablesPage.DataContext = this.MainVM; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
