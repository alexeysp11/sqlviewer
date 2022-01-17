using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for TextFileWindow.xaml
    /// </summary>
    public partial class TextFileWindow : Window
    {
        private MainVM MainVM { get; set; }

        public TextFileWindow()
        {
            InitializeComponent();
        }
    }
}
