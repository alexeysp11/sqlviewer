using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SqlViewer.UserControls;

public partial class DbGrid : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty
        = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(DbGrid),
            new PropertyMetadata(null, OnItemsSourceChanged));

    public static readonly DependencyProperty IsReadOnlyProperty
        = DependencyProperty.Register(
            nameof(IsReadOnly),
            typeof(bool),
            typeof(DbGrid),
            new PropertyMetadata(false, OnIsReadOnlyChanged));

    public static readonly DependencyProperty AutoGenerateColumnsProperty
        = DependencyProperty.Register(
            nameof(AutoGenerateColumns),
            typeof(bool),
            typeof(DbGrid),
            new PropertyMetadata(false, OnAutoGenerateColumnsChanged));

    public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }
    public bool IsReadOnly { get => (bool)GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }
    public bool AutoGenerateColumns { get => (bool)GetValue(AutoGenerateColumnsProperty); set => SetValue(AutoGenerateColumnsProperty, value); }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DbGrid control) control.dgrBrowse.ItemsSource = e.NewValue as IEnumerable;
    }

    private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DbGrid control) control.dgrBrowse.IsReadOnly = (bool)e.NewValue;
    }

    private static void OnAutoGenerateColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DbGrid control) control.dgrBrowse.AutoGenerateColumns = (bool)e.NewValue;
    }

    public DbGrid()
    {
        InitializeComponent();
    }

    private void dgrBrowse_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        string header = e.Column.Header?.ToString();
        if (header != null) e.Column.Header = header.Replace("_", "__");
    }
}
