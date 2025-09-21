using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Pages
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
