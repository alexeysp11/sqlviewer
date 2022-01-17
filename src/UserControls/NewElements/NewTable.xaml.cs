using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.NewElements
{
    /// <summary>
    /// Interaction logic for NewTable.xaml
    /// </summary>
    public partial class NewTable : UserControl
    {
        private MainVM MainVM { get; set; }

        public NewTable()
        {
            InitializeComponent();
        }
    }
}
