using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    public partial class TablesPage : UserControl
    {
        private MainVM MainVM { get; set; }

        public TablesPage()
        {
            InitializeComponent();
            
            Loaded += (o, e) => this.MainVM = (MainVM)(this.DataContext); 
        }

        private void ResultDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", "__");
        }

        private void SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (e.NewValue == null) 
                    return; 
                string selectedItem = (e.NewValue).ToString(); 
                foreach (TreeViewItem tableName in this.tvTables.Items)
                {
                    if (tableName.ToString() == selectedItem)
                    {
                        this.tbTableName.Text = tableName.Header.ToString(); 
                        
                        this.MainVM.GetAllDataFromTable(tableName.Header.ToString()); 
                        this.MainVM.GetColumnsOfTable(tableName.Header.ToString()); 
                        this.MainVM.GetForeignKeys(tableName.Header.ToString()); 
                        this.MainVM.GetTriggers(tableName.Header.ToString()); 
                        this.MainVM.GetSqlDefinition(tableName.Header.ToString()); 
                        break; 
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
