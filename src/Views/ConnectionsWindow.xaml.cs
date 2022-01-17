using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionsWindow.xaml
    /// </summary>
    public partial class ConnectionsWindow : Window
    {
        private MainVM MainVM { get; set; }

        public ConnectionsWindow()
        {
            InitializeComponent();
        }
    }
}
