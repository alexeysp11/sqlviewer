using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.Views
{
    /// <summary>
    /// Interaction logic for TextFileView.xaml
    /// </summary>
    public partial class TextFileView : Window
    {
        private MainVM MainVM { get; set; }

        public TextFileView()
        {
            InitializeComponent();
        }
    }
}
