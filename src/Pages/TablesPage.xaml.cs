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
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private TablesPageEntity TablesPageEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            tbTablesPageDb.Text = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? (RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.PathField.Translation) ? TablesPageEntity.PathField.English + ": " : TablesPageEntity.PathField.Translation + ": ") : "DB: ";
            tbTables.Text = SettingsHelper.TranslateUiElement(TablesPageEntity.TablesField.English, TablesPageEntity.TablesField.Translation); 
            tblDbName.Text = RepoHelper.AppSettingsRepo.DbName; 
            lblGeneralInfo.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.GeneralInfoField.English, TablesPageEntity.GeneralInfoField.Translation); 
            lblColumns.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.ColumnsField.English, TablesPageEntity.ColumnsField.Translation); 
            lblForeignKeys.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.ForeignKeysField.English, TablesPageEntity.ForeignKeysField.Translation); 
            lblTriggers.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.TriggersField.English, TablesPageEntity.TriggersField.Translation); 
            lblData.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.DataField.English, TablesPageEntity.DataField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResultDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", "__");
        }

        /// <summary>
        /// 
        /// </summary>
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
                        
                        this.MainVM.DataVM.MainDbBranch.GetAllDataFromTable(tableName.Header.ToString()); 
                        this.MainVM.DataVM.MainDbBranch.GetColumnsOfTable(tableName.Header.ToString()); 
                        this.MainVM.DataVM.MainDbBranch.GetForeignKeys(tableName.Header.ToString()); 
                        this.MainVM.DataVM.MainDbBranch.GetTriggers(tableName.Header.ToString()); 
                        this.MainVM.DataVM.MainDbBranch.GetSqlDefinition(tableName.Header.ToString()); 
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
