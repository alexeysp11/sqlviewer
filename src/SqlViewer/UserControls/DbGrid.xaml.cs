using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SqlViewer.UserControls;

/// <summary>
/// Interaction logic for DbGrid.xaml
/// </summary>
public partial class DbGrid : UserControl
{
    public static readonly DependencyProperty IsReadOnlyProperty
        = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DbGrid),
            new PropertyMetadata(false));
    
    public static readonly DependencyProperty AutoGenerateColumnsProperty
        = DependencyProperty.Register("AutoGenerateColumns", typeof(bool), typeof(DbGrid),
            new PropertyMetadata(false));

    public static readonly DependencyProperty ItemsSourceProperty
        = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DbGrid),
            new PropertyMetadata(null));
    
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set 
        {
            SetValue(IsReadOnlyProperty, value);
            dgrBrowse.IsReadOnly = value;
        }
    }

    public bool AutoGenerateColumns
    {
        get => (bool)GetValue(AutoGenerateColumnsProperty);
        set 
        {
            SetValue(AutoGenerateColumnsProperty, value);
            dgrBrowse.AutoGenerateColumns = value;
        }
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set 
        {
            SetValue(ItemsSourceProperty, value);
            dgrBrowse.ItemsSource = value;
        }
    }

    public DbGrid()
    {
        InitializeComponent();

        Loaded += (o, e) => 
        {
            grBrowse.Height = Height;
            grBrowse.Width = Width;

            dgrBrowse.AutoGenerateColumns = AutoGenerateColumns;
            dgrBrowse.IsReadOnly = IsReadOnly;
            dgrBrowse.ItemsSource = ItemsSource;
        };
    }

    private void dgrBrowse_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        string header = e.Column.Header.ToString();
        e.Column.Header = header.Replace("_", "__");
    }
}
