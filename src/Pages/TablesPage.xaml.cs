using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.PagesEntities; 
using SqlViewer.Helpers; 
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
                this.MainVM.VisualVM.TablesPage = this; 
                this.TablesPageEntity = this.MainVM.Translator.TablesPageEntity;  
                Init(); 
            }; 
        }

        public void Init()
        {
            tbPath.Text = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.PathField.Translation) ? TablesPageEntity.PathField.English + ": " : TablesPageEntity.PathField.Translation + ": ";
            tbTables.Text = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.TablesField.Translation) ? TablesPageEntity.TablesField.English : TablesPageEntity.TablesField.Translation;
            tblDbName.Text = RepoHelper.AppSettingsRepo.DbName; 
            lblGeneralInfo.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.GeneralInfoField.Translation) ? TablesPageEntity.GeneralInfoField.English : TablesPageEntity.GeneralInfoField.Translation; 
            lblColumns.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.ColumnsField.Translation) ? TablesPageEntity.ColumnsField.English : TablesPageEntity.ColumnsField.Translation; 
            lblForeignKeys.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.ForeignKeysField.Translation) ? TablesPageEntity.ForeignKeysField.English : TablesPageEntity.ForeignKeysField.Translation; 
            lblTriggers.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.TriggersField.Translation) ? TablesPageEntity.TriggersField.English : TablesPageEntity.TriggersField.Translation; 
            lblData.Content = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.DataField.Translation) ? TablesPageEntity.DataField.English : TablesPageEntity.DataField.Translation; 
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
                        
                        this.MainVM.DataVM.GetAllDataFromTable(tableName.Header.ToString()); 
                        this.MainVM.DataVM.GetColumnsOfTable(tableName.Header.ToString()); 
                        this.MainVM.DataVM.GetForeignKeys(tableName.Header.ToString()); 
                        this.MainVM.DataVM.GetTriggers(tableName.Header.ToString()); 
                        this.MainVM.DataVM.GetSqlDefinition(tableName.Header.ToString()); 
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
