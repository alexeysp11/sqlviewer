using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;
using SqlViewer.Entities.UserControlsEntities; 
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
                this.MainVM.Menu = this; 
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
            miFile.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileField.Translation) ? MenuEntity.FileField.English : MenuEntity.FileField.Translation; 

            miFileNew.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewField.Translation) ? MenuEntity.FileNewField.English : MenuEntity.FileNewField.Translation; 
            miFileNewSQL.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewSqlFileField.Translation) ? MenuEntity.FileNewSqlFileField.English : MenuEntity.FileNewSqlFileField.Translation; 
            miFileNewFunction.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewFunctionField.Translation) ? MenuEntity.FileNewFunctionField.English : MenuEntity.FileNewFunctionField.Translation; 
            miFileNewFunction.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewProcedure.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewProcedureField.Translation) ? MenuEntity.FileNewProcedureField.English : MenuEntity.FileNewProcedureField.Translation; 
            miFileNewProcedure.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewTest.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewTestField.Translation) ? MenuEntity.FileNewTestField.English : MenuEntity.FileNewTestField.Translation; 
            miFileNewTest.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileNewDatabase.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewDatabaseField.Translation) ? MenuEntity.FileNewDatabaseField.English : MenuEntity.FileNewDatabaseField.Translation;  
            miFileNewTable.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewTableField.Translation) ? MenuEntity.FileNewTableField.English : MenuEntity.FileNewTableField.Translation;  
            miFileNewSequence.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewSequenceField.Translation) ? MenuEntity.FileNewSequenceField.English : MenuEntity.FileNewSequenceField.Translation;  
            miFileNewView.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewViewField.Translation) ? MenuEntity.FileNewViewField.English : MenuEntity.FileNewViewField.Translation;  
            miFileNewTrigger.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileNewTriggerField.Translation) ? MenuEntity.FileNewTriggerField.English : MenuEntity.FileNewTriggerField.Translation;  

            miFileOpen.Header = MenuEntity.FileOpenField.Translation; 
            miFileOpenSQL.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenSqlFileField.Translation) ? MenuEntity.FileOpenSqlFileField.English : MenuEntity.FileOpenSqlFileField.Translation;  
            miFileOpenFunction.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenFunctionField.Translation) ? MenuEntity.FileOpenFunctionField.English : MenuEntity.FileOpenFunctionField.Translation;  
            miFileOpenFunction.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenProcedure.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenProcedureField.Translation) ? MenuEntity.FileOpenProcedureField.English : MenuEntity.FileOpenProcedureField.Translation;  
            miFileOpenProcedure.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenTest.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenTestField.Translation) ? MenuEntity.FileOpenTestField.English : MenuEntity.FileOpenTestField.Translation;  
            miFileOpenTest.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miFileOpenDatabase.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenDatabaseField.Translation) ? MenuEntity.FileOpenDatabaseField.English : MenuEntity.FileOpenDatabaseField.Translation;  
            miFileOpenTable.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenTableField.Translation) ? MenuEntity.FileOpenTableField.English : MenuEntity.FileOpenTableField.Translation;  
            miFileOpenSequence.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenSequenceField.Translation) ? MenuEntity.FileOpenSequenceField.English : MenuEntity.FileOpenSequenceField.Translation;  
            miFileOpenView.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenViewField.Translation) ? MenuEntity.FileOpenViewField.English : MenuEntity.FileOpenViewField.Translation;  
            miFileOpenTrigger.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileOpenTriggerField.Translation) ? MenuEntity.FileOpenTriggerField.English : MenuEntity.FileOpenTriggerField.Translation;  

            miFileReopen.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileReopenField.Translation) ? MenuEntity.FileReopenField.English : MenuEntity.FileReopenField.Translation;  
            miFileSave.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileSaveField.Translation) ? MenuEntity.FileSaveField.English : MenuEntity.FileSaveField.Translation;  
            miFileSaveAll.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileSaveAllField.Translation) ? MenuEntity.FileSaveAllField.English : MenuEntity.FileSaveAllField.Translation; 
            miFileClose.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileCloseField.Translation) ? MenuEntity.FileCloseField.English : MenuEntity.FileCloseField.Translation;  
            miFileCloseAll.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileCloseAllField.Translation) ? MenuEntity.FileCloseAllField.English : MenuEntity.FileCloseAllField.Translation; 
            miFileExit.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.FileExitField.Translation) ? MenuEntity.FileExitField.English : MenuEntity.FileExitField.Translation;  
        }

        private void InitEdit()
        {
            miEdit.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.EditField.Translation) ? MenuEntity.EditField.English : MenuEntity.EditField.Translation;  
            miEditUndo.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.EditUndoField.Translation) ? MenuEntity.EditUndoField.English : MenuEntity.EditUndoField.Translation;  
            miEditRedo.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.EditRedoField.Translation) ? MenuEntity.EditRedoField.English : MenuEntity.EditRedoField.Translation;  
            miEditSettings.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.EditSettingsField.Translation) ? MenuEntity.EditSettingsField.English : MenuEntity.EditSettingsField.Translation;  
        }

        private void InitPages()
        {
            miPages.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesField.Translation) ? MenuEntity.PagesField.English : MenuEntity.PagesField.Translation;  
            miPagesSqlQuery.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesSqlQueryField.Translation) ? MenuEntity.PagesSqlQueryField.English : MenuEntity.PagesSqlQueryField.Translation; 
            miPagesCommandLine.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesCommandLineField.Translation) ? MenuEntity.PagesCommandLineField.English : MenuEntity.PagesCommandLineField.Translation; 
            miPagesFunctions.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesFunctionsField.Translation) ? MenuEntity.PagesFunctionsField.English : MenuEntity.PagesFunctionsField.Translation;  
            miPagesFunctions.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesProcedures.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesProceduresField.Translation) ? MenuEntity.PagesProceduresField.English : MenuEntity.PagesProceduresField.Translation;  
            miPagesProcedures.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesTests.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesTestsField.Translation) ? MenuEntity.PagesTestsField.English : MenuEntity.PagesTestsField.Translation; 
            miPagesTests.IsEnabled = this.MainVM.AppRepository.ActiveRdbms == RdbmsEnum.SQLite ? false : true;
            miPagesDatabases.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesDatabasesField.Translation) ? MenuEntity.PagesDatabasesField.English : MenuEntity.PagesDatabasesField.Translation;  
            miPagesTables.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.PagesTablesField.Translation) ? MenuEntity.PagesTablesField.English : MenuEntity.PagesTablesField.Translation;  
        }

        private void InitTools()
        {
            miTools.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.ToolsField.Translation) ? MenuEntity.ToolsField.English : MenuEntity.ToolsField.Translation;  
            miToolsOptions.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.ToolsOptionsField.Translation) ? MenuEntity.ToolsOptionsField.English : MenuEntity.ToolsOptionsField.Translation;  
            miToolsToolbars.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.ToolsToolbarsField.Translation) ? MenuEntity.ToolsToolbarsField.English : MenuEntity.ToolsToolbarsField.Translation;  
            miToolsConnections.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.ToolsConnectionsField.Translation) ? MenuEntity.ToolsConnectionsField.English : MenuEntity.ToolsConnectionsField.Translation;  
        }

        private void InitHelp()
        {
            miHelp.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.HelpField.Translation) ? MenuEntity.HelpField.English : MenuEntity.HelpField.Translation;  
            miHelpDocs.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.HelpDocsField.Translation) ? MenuEntity.HelpDocsField.English : MenuEntity.HelpDocsField.Translation;  
            miHelpDocsSqlite.Header = "SQLite"; 
            miHelpDocsPostgres.Header = "PostgreSQL"; 
            miHelpDocsMySql.Header = "MySQL"; 
            miHelpUserGuide.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.HelpUserGuideField.Translation) ? MenuEntity.HelpUserGuideField.English : MenuEntity.HelpUserGuideField.Translation; 
            miHelpUserGuideAbout.Header = "About"; 
            miHelpGithub.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.HelpGithubField.Translation) ? MenuEntity.HelpGithubField.English : MenuEntity.HelpGithubField.Translation; 
            miHelpReport.Header = this.MainVM.AppRepository.Language == LanguageEnum.English || System.String.IsNullOrEmpty(MenuEntity.HelpReportField.Translation) ? MenuEntity.HelpReportField.English : MenuEntity.HelpReportField.Translation;  
        }
    }
}
