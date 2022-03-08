using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.PagesEntities; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Pages
{
    /// <summary>
    /// Interaction logic for TablesPage.xaml
    /// </summary>
    public partial class TablesPage : UserControl
    {
        private MainVM MainVM { get; set; }
        private TablesPageEntity TablesPageEntity { get; set; }

        public TablesPage()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.TablesPage = this; 
                this.TablesPageEntity = this.MainVM.Translator.TablesPageEntity;  
                Init(); 
            }; 
        }

        public void Init()
        {
            tbPath.Text = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.PathField.Translation) ? TablesPageEntity.PathField.English + ": " : TablesPageEntity.PathField.Translation + ": ";
            tbTables.Text = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.TablesField.Translation) ? TablesPageEntity.TablesField.English : TablesPageEntity.TablesField.Translation;
            lblGeneralInfo.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.GeneralInfoField.Translation) ? TablesPageEntity.GeneralInfoField.English : TablesPageEntity.GeneralInfoField.Translation; 
            lblColumns.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.ColumnsField.Translation) ? TablesPageEntity.ColumnsField.English : TablesPageEntity.ColumnsField.Translation; 
            lblForeignKeys.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.ForeignKeysField.Translation) ? TablesPageEntity.ForeignKeysField.English : TablesPageEntity.ForeignKeysField.Translation; 
            lblTriggers.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.TriggersField.Translation) ? TablesPageEntity.TriggersField.English : TablesPageEntity.TriggersField.Translation; 
            lblData.Content = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(TablesPageEntity.DataField.Translation) ? TablesPageEntity.DataField.English : TablesPageEntity.DataField.Translation; 
        }

        private void ResultDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            System.String header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", "__");
        }

        private void SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (e.NewValue == null) 
                    return; 
                System.String selectedItem = (e.NewValue).ToString(); 
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
