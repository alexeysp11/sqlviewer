using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for ProceduresPage.xaml
    /// </summary>
    public partial class ProceduresPage : UserControl
    {
        private MainVM MainVM { get; set; }

        public ProceduresPage()
        {
            InitializeComponent();
        }
    }
}
