using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.UserControlsEntities; 
using SqlViewer.Helpers; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.UserControls
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private MenuEntity MenuEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Menu()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainVM = (MainVM)this.DataContext; 
                this.MainVM.VisualVM.Menu = this; 
                this.MenuEntity = this.MainVM.Translator.MenuEntity;  
                Init(); 
            }; 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            InitFile(); 
            InitEdit(); 
            InitPages(); 
            InitTools(); 
            InitHelp(); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitFile()
        {
            miFile.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileField.English, MenuEntity.FileField.Translation); 

            miFileNew.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewField.English, MenuEntity.FileNewField.Translation); 
            miFileNewSQL.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewSqlFileField.English, MenuEntity.FileNewSqlFileField.Translation); 
            miFileNewFunction.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewFunctionField.English, MenuEntity.FileNewFunctionField.Translation); 
            miFileNewFunction.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewProcedure.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewProcedureField.English, MenuEntity.FileNewProcedureField.Translation); 
            miFileNewProcedure.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewTest.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewTestField.English, MenuEntity.FileNewTestField.Translation); 
            miFileNewTest.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewDatabase.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewDatabaseField.English, MenuEntity.FileNewDatabaseField.Translation); 
            miFileNewTable.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewTableField.English, MenuEntity.FileNewTableField.Translation); 
            miFileNewTable.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewSequence.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewSequenceField.English, MenuEntity.FileNewSequenceField.Translation); 
            miFileNewSequence.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewView.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewViewField.English, MenuEntity.FileNewViewField.Translation); 
            miFileNewView.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewTrigger.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileNewTriggerField.English, MenuEntity.FileNewTriggerField.Translation); 
            miFileNewTrigger.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;

            miFileOpen.Header = MenuEntity.FileOpenField.Translation; 
            miFileOpenSQL.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenSqlFileField.English, MenuEntity.FileOpenSqlFileField.Translation); 
            miFileOpenFunction.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenFunctionField.English, MenuEntity.FileOpenFunctionField.Translation); 
            miFileOpenFunction.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenProcedure.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenProcedureField.English, MenuEntity.FileOpenProcedureField.Translation); 
            miFileOpenProcedure.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenTest.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenTestField.English, MenuEntity.FileOpenTestField.Translation); 
            miFileOpenTest.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenDatabase.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenDatabaseField.English, MenuEntity.FileOpenDatabaseField.Translation); 
            miFileOpenTable.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenTableField.English, MenuEntity.FileOpenTableField.Translation); 
            miFileOpenTable.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenSequence.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenSequenceField.English, MenuEntity.FileOpenSequenceField.Translation); 
            miFileOpenSequence.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenView.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenViewField.English, MenuEntity.FileOpenViewField.Translation); 
            miFileOpenView.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenTrigger.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileOpenTriggerField.English, MenuEntity.FileOpenTriggerField.Translation); 
            miFileOpenTrigger.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;

            miFileReopen.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileReopenField.English, MenuEntity.FileReopenField.Translation); 
            miFileSave.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileSaveField.English, MenuEntity.FileSaveField.Translation); 
            miFileSaveAll.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileSaveAllField.English, MenuEntity.FileSaveAllField.Translation); 
            miFileClose.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileCloseField.English, MenuEntity.FileCloseField.Translation); 
            miFileCloseAll.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileCloseAllField.English, MenuEntity.FileCloseAllField.Translation); 
            miFileExit.Header = SettingsHelper.TranslateUiElement(MenuEntity.FileExitField.English, MenuEntity.FileExitField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitEdit()
        {
            miEdit.Header = SettingsHelper.TranslateUiElement(MenuEntity.EditField.English, MenuEntity.EditField.Translation); 
            miEditUndo.Header = SettingsHelper.TranslateUiElement(MenuEntity.EditUndoField.English, MenuEntity.EditUndoField.Translation); 
            miEditRedo.Header = SettingsHelper.TranslateUiElement(MenuEntity.EditRedoField.English, MenuEntity.EditRedoField.Translation); 
            miEditSettings.Header = SettingsHelper.TranslateUiElement(MenuEntity.EditSettingsField.English, MenuEntity.EditSettingsField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPages()
        {
            miPages.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesField.English, MenuEntity.PagesField.Translation); 
            miPagesSqlQuery.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesSqlQueryField.English, MenuEntity.PagesSqlQueryField.Translation); 
            miPagesCommandLine.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesCommandLineField.English, MenuEntity.PagesCommandLineField.Translation); 
            miPagesFunctions.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesFunctionsField.English, MenuEntity.PagesFunctionsField.Translation); 
            miPagesFunctions.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesProcedures.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesProceduresField.English, MenuEntity.PagesProceduresField.Translation); 
            miPagesProcedures.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesTests.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesTestsField.English, MenuEntity.PagesTestsField.Translation); 
            miPagesTests.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesDatabases.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesDatabasesField.English, MenuEntity.PagesDatabasesField.Translation); 
            miPagesDatabases.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesTables.Header = SettingsHelper.TranslateUiElement(MenuEntity.PagesTablesField.English, MenuEntity.PagesTablesField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitTools()
        {
            miTools.Header = SettingsHelper.TranslateUiElement(MenuEntity.ToolsField.English, MenuEntity.ToolsField.Translation); 
            miToolsOptions.Header = SettingsHelper.TranslateUiElement(MenuEntity.ToolsOptionsField.English, MenuEntity.ToolsOptionsField.Translation); 
            miToolsToolbars.Header = SettingsHelper.TranslateUiElement(MenuEntity.ToolsToolbarsField.English, MenuEntity.ToolsToolbarsField.Translation); 
            miToolsConnections.Header = SettingsHelper.TranslateUiElement(MenuEntity.ToolsConnectionsField.English, MenuEntity.ToolsConnectionsField.Translation); 
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitHelp()
        {
            miHelp.Header = SettingsHelper.TranslateUiElement(MenuEntity.HelpField.English, MenuEntity.HelpField.Translation); 
            miHelpDocs.Header = SettingsHelper.TranslateUiElement(MenuEntity.HelpDocsField.English, MenuEntity.HelpDocsField.Translation); 
            miHelpDocsSqlite.Header = "SQLite"; 
            miHelpDocsPostgres.Header = "PostgreSQL"; 
            miHelpDocsMySql.Header = "MySQL"; 
            miHelpUserGuide.Header = SettingsHelper.TranslateUiElement(MenuEntity.HelpUserGuideField.English, MenuEntity.HelpUserGuideField.Translation); 
            miHelpUserGuideAbout.Header = "About"; 
            miHelpGithub.Header = SettingsHelper.TranslateUiElement(MenuEntity.HelpGithubField.English, MenuEntity.HelpGithubField.Translation); 
            miHelpReport.Header = SettingsHelper.TranslateUiElement(MenuEntity.HelpReportField.English, MenuEntity.HelpReportField.Translation); 
        }
    }
}
