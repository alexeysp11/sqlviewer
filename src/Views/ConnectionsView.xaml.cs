using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionsView.xaml
    /// </summary>
    public partial class ConnectionsView : Window
    {
        private MainVM MainVM { get; set; }

        public ConnectionsView()
        {
            InitializeComponent();
        }
    }
}
