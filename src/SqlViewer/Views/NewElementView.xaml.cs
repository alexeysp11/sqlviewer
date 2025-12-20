using System.Windows;
using SqlViewer.ViewModels;

namespace SqlViewer.Views;

/// <summary>
/// Interaction logic for NewElementView.xaml
/// </summary>
public partial class NewElementView : Window
{
    private MainVM MainVM { get; set; }

    public NewElementView()
    {
        InitializeComponent();
    }
}
