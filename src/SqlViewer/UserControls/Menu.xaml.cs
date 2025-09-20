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
        private MainVM MainVM { get; set; }
        private MenuEntity MenuEntity { get; set; }

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

        public void Init()
        {
            InitFile(); 
            InitEdit(); 
            InitPages(); 
            InitTools(); 
            InitHelp(); 
        }

        private void InitFile()
        {
            miFile.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileField.Translation) ? MenuEntity.FileField.English : MenuEntity.FileField.Translation; 

            miFileNew.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewField.Translation) ? MenuEntity.FileNewField.English : MenuEntity.FileNewField.Translation; 
            miFileNewSQL.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewSqlFileField.Translation) ? MenuEntity.FileNewSqlFileField.English : MenuEntity.FileNewSqlFileField.Translation; 
            miFileNewFunction.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewFunctionField.Translation) ? MenuEntity.FileNewFunctionField.English : MenuEntity.FileNewFunctionField.Translation; 
            miFileNewFunction.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewProcedure.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewProcedureField.Translation) ? MenuEntity.FileNewProcedureField.English : MenuEntity.FileNewProcedureField.Translation; 
            miFileNewProcedure.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewTest.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewTestField.Translation) ? MenuEntity.FileNewTestField.English : MenuEntity.FileNewTestField.Translation; 
            miFileNewTest.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewDatabase.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewDatabaseField.Translation) ? MenuEntity.FileNewDatabaseField.English : MenuEntity.FileNewDatabaseField.Translation;  
            miFileNewTable.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewTableField.Translation) ? MenuEntity.FileNewTableField.English : MenuEntity.FileNewTableField.Translation;  
            miFileNewTable.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewSequence.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewSequenceField.Translation) ? MenuEntity.FileNewSequenceField.English : MenuEntity.FileNewSequenceField.Translation;  
            miFileNewSequence.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewView.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewViewField.Translation) ? MenuEntity.FileNewViewField.English : MenuEntity.FileNewViewField.Translation;  
            miFileNewView.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileNewTrigger.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileNewTriggerField.Translation) ? MenuEntity.FileNewTriggerField.English : MenuEntity.FileNewTriggerField.Translation;  
            miFileNewTrigger.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;

            miFileOpen.Header = MenuEntity.FileOpenField.Translation; 
            miFileOpenSQL.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenSqlFileField.Translation) ? MenuEntity.FileOpenSqlFileField.English : MenuEntity.FileOpenSqlFileField.Translation;  
            miFileOpenFunction.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenFunctionField.Translation) ? MenuEntity.FileOpenFunctionField.English : MenuEntity.FileOpenFunctionField.Translation;  
            miFileOpenFunction.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenProcedure.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenProcedureField.Translation) ? MenuEntity.FileOpenProcedureField.English : MenuEntity.FileOpenProcedureField.Translation;  
            miFileOpenProcedure.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenTest.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenTestField.Translation) ? MenuEntity.FileOpenTestField.English : MenuEntity.FileOpenTestField.Translation;  
            miFileOpenTest.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenDatabase.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenDatabaseField.Translation) ? MenuEntity.FileOpenDatabaseField.English : MenuEntity.FileOpenDatabaseField.Translation;  
            miFileOpenTable.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenTableField.Translation) ? MenuEntity.FileOpenTableField.English : MenuEntity.FileOpenTableField.Translation;  
            miFileOpenTable.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenSequence.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenSequenceField.Translation) ? MenuEntity.FileOpenSequenceField.English : MenuEntity.FileOpenSequenceField.Translation;  
            miFileOpenSequence.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenView.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenViewField.Translation) ? MenuEntity.FileOpenViewField.English : MenuEntity.FileOpenViewField.Translation;  
            miFileOpenView.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;
            miFileOpenTrigger.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileOpenTriggerField.Translation) ? MenuEntity.FileOpenTriggerField.English : MenuEntity.FileOpenTriggerField.Translation;  
            miFileOpenTrigger.IsEnabled = string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName) ? false : true;

            miFileReopen.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileReopenField.Translation) ? MenuEntity.FileReopenField.English : MenuEntity.FileReopenField.Translation;  
            miFileSave.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileSaveField.Translation) ? MenuEntity.FileSaveField.English : MenuEntity.FileSaveField.Translation;  
            miFileSaveAll.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileSaveAllField.Translation) ? MenuEntity.FileSaveAllField.English : MenuEntity.FileSaveAllField.Translation; 
            miFileClose.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileCloseField.Translation) ? MenuEntity.FileCloseField.English : MenuEntity.FileCloseField.Translation;  
            miFileCloseAll.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileCloseAllField.Translation) ? MenuEntity.FileCloseAllField.English : MenuEntity.FileCloseAllField.Translation; 
            miFileExit.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.FileExitField.Translation) ? MenuEntity.FileExitField.English : MenuEntity.FileExitField.Translation;  
        }

        private void InitEdit()
        {
            miEdit.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.EditField.Translation) ? MenuEntity.EditField.English : MenuEntity.EditField.Translation;  
            miEditUndo.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.EditUndoField.Translation) ? MenuEntity.EditUndoField.English : MenuEntity.EditUndoField.Translation;  
            miEditRedo.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.EditRedoField.Translation) ? MenuEntity.EditRedoField.English : MenuEntity.EditRedoField.Translation;  
            miEditSettings.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.EditSettingsField.Translation) ? MenuEntity.EditSettingsField.English : MenuEntity.EditSettingsField.Translation;  
        }

        private void InitPages()
        {
            miPages.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesField.Translation) ? MenuEntity.PagesField.English : MenuEntity.PagesField.Translation;  
            miPagesSqlQuery.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesSqlQueryField.Translation) ? MenuEntity.PagesSqlQueryField.English : MenuEntity.PagesSqlQueryField.Translation; 
            miPagesCommandLine.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesCommandLineField.Translation) ? MenuEntity.PagesCommandLineField.English : MenuEntity.PagesCommandLineField.Translation; 
            miPagesFunctions.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesFunctionsField.Translation) ? MenuEntity.PagesFunctionsField.English : MenuEntity.PagesFunctionsField.Translation;  
            miPagesFunctions.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesProcedures.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesProceduresField.Translation) ? MenuEntity.PagesProceduresField.English : MenuEntity.PagesProceduresField.Translation;  
            miPagesProcedures.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesTests.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesTestsField.Translation) ? MenuEntity.PagesTestsField.English : MenuEntity.PagesTestsField.Translation; 
            miPagesTests.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesDatabases.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesDatabasesField.Translation) ? MenuEntity.PagesDatabasesField.English : MenuEntity.PagesDatabasesField.Translation;  
            miPagesDatabases.IsEnabled = RepoHelper.AppSettingsRepo.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesTables.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.PagesTablesField.Translation) ? MenuEntity.PagesTablesField.English : MenuEntity.PagesTablesField.Translation;  
        }

        private void InitTools()
        {
            miTools.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.ToolsField.Translation) ? MenuEntity.ToolsField.English : MenuEntity.ToolsField.Translation;  
            miToolsOptions.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.ToolsOptionsField.Translation) ? MenuEntity.ToolsOptionsField.English : MenuEntity.ToolsOptionsField.Translation;  
            miToolsToolbars.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.ToolsToolbarsField.Translation) ? MenuEntity.ToolsToolbarsField.English : MenuEntity.ToolsToolbarsField.Translation;  
            miToolsConnections.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.ToolsConnectionsField.Translation) ? MenuEntity.ToolsConnectionsField.English : MenuEntity.ToolsConnectionsField.Translation;  
        }

        private void InitHelp()
        {
            miHelp.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.HelpField.Translation) ? MenuEntity.HelpField.English : MenuEntity.HelpField.Translation;  
            miHelpDocs.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.HelpDocsField.Translation) ? MenuEntity.HelpDocsField.English : MenuEntity.HelpDocsField.Translation;  
            miHelpDocsSqlite.Header = "SQLite"; 
            miHelpDocsPostgres.Header = "PostgreSQL"; 
            miHelpDocsMySql.Header = "MySQL"; 
            miHelpUserGuide.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.HelpUserGuideField.Translation) ? MenuEntity.HelpUserGuideField.English : MenuEntity.HelpUserGuideField.Translation; 
            miHelpUserGuideAbout.Header = "About"; 
            miHelpGithub.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.HelpGithubField.Translation) ? MenuEntity.HelpGithubField.English : MenuEntity.HelpGithubField.Translation; 
            miHelpReport.Header = RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(MenuEntity.HelpReportField.Translation) ? MenuEntity.HelpReportField.English : MenuEntity.HelpReportField.Translation;  
        }
    }
}
