using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for FunctionsPage.xaml
    /// </summary>
    public partial class FunctionsPage : UserControl
    {
        private MainVM MainVM { get; set; }

        public FunctionsPage()
        {
            InitializeComponent();
        }
    }
}
