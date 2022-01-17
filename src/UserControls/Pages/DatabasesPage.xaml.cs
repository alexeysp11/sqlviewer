using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for DatabasesPage.xaml
    /// </summary>
    public partial class DatabasesPage : UserControl
    {
        private MainVM MainVM { get; set; }

        public DatabasesPage()
        {
            InitializeComponent();
        }
    }
}
