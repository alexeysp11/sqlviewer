using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.NewElements
{
    /// <summary>
    /// Interaction logic for NewDatabase.xaml
    /// </summary>
    public partial class NewDatabase : UserControl
    {
        private MainVM MainVM { get; set; }

        public NewDatabase()
        {
            InitializeComponent();
        }
    }
}
