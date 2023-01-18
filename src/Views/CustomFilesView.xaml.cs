using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for CustomFilesView.xaml
    /// </summary>
    public partial class CustomFilesView : Window
    {
        private MainVM MainVM { get; set; }

        public CustomFilesView()
        {
            InitializeComponent();
        }
    }
}
