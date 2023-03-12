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
        /// Main ViewModel 
        /// </summary>
        private MainVM MainVM { get; set; }
        /// <summary>
        /// Entity that stores data for translating the page 
        /// </summary>
        private TablesPageEntity TablesPageEntity { get; set; }

        /// <summary>
        /// Constructor of TablesPage
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
        /// Initializes all the page 
        /// </summary>
        public void Init()
        {
            tbTablesPageDb.Text = RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms == RdbmsEnum.SQLite ? (RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(TablesPageEntity.PathField.Translation) ? TablesPageEntity.PathField.English + ": " : TablesPageEntity.PathField.Translation + ": ") : "DB: ";
            tbTables.Text = SettingsHelper.TranslateUiElement(TablesPageEntity.TablesField.English, TablesPageEntity.TablesField.Translation); 
            tblDbName.Text = RepoHelper.AppSettingsRepo.DatabaseSettings.DbName; 
            lblGeneralInfo.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.GeneralInfoField.English, TablesPageEntity.GeneralInfoField.Translation); 
            lblColumns.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.ColumnsField.English, TablesPageEntity.ColumnsField.Translation); 
            lblForeignKeys.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.ForeignKeysField.English, TablesPageEntity.ForeignKeysField.Translation); 
            lblTriggers.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.TriggersField.English, TablesPageEntity.TriggersField.Translation); 
            lblData.Content = SettingsHelper.TranslateUiElement(TablesPageEntity.DataField.English, TablesPageEntity.DataField.Translation); 
        }

        /// <summary>
        /// Displays headers of in the DataGrid correctly 
        /// </summary>
        private void ResultDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", "__");
        }

        /// <summary>
        /// Displays information about selected table inside TableView 
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
                    string tn = tableName.ToString(); 
                    if (tn == selectedItem)
                    {
                        this.tbTableName.Text = tn; 
                        this.MainVM.VisualVM.DisplayAllDataFromTable(tn); 
                        this.MainVM.VisualVM.DisplayColumnsOfTable(tn); 
                        this.MainVM.VisualVM.DisplayForeignKeys(tn); 
                        this.MainVM.VisualVM.DisplayTriggers(tn); 
                        this.MainVM.VisualVM.DisplayTableDefinition(tn); 
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
