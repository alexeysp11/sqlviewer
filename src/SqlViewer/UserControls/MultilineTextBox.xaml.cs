using System.Windows;
using System.Windows.Controls;

namespace SqlViewer.UserControls;

/// <summary>
/// Interaction logic for MultilineTextBox.xaml
/// </summary>
public partial class MultilineTextBox : UserControl
{
    public static readonly DependencyProperty IsReadOnlyProperty
        = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultilineTextBox),
            new PropertyMetadata(false));

    public static readonly DependencyProperty TextProperty
        = DependencyProperty.Register("Text", typeof(string), typeof(MultilineTextBox),
            new PropertyMetadata(string.Empty));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set
        {
            SetValue(IsReadOnlyProperty, value);
            tbMultiline.IsReadOnly = value;
        }
    }

    public string Text
    {
        get
        {
            SetValue(TextProperty, tbMultiline.Text);
            return (string)GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
            tbMultiline.Text = value;
        }
    }

    public MultilineTextBox()
    {
        InitializeComponent();

        Loaded += (o, e) =>
        {
            tbMultiline.MinHeight = Height;
            tbMultiline.MaxHeight = Height;
            brdMultiline.Height = Height;

            tbMultiline.MinWidth = Width;
            tbMultiline.MaxWidth = Width;
            brdMultiline.Width = Width;

            tbMultiline.IsReadOnly = IsReadOnly;
            tbMultiline.Text = Text;
            tbMultiline.FontSize = FontSize;
        };
    }
}
