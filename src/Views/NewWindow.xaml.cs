using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        private MainVM MainVM { get; set; }

        public NewWindow()
        {
            InitializeComponent();
        }
    }
}
