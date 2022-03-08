using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.UserControlsEntities
{
    public class MenuEntity 
    {
        #region File fields
        public Word FileField { get; private set; } = new Word("File"); 

        public Word FileNewField { get; private set; } = new Word("New"); 
        public Word FileNewSqlFileField { get; private set; } = new Word("SQL file"); 
        public Word FileNewFunctionField { get; private set; } = new Word("Function"); 
        public Word FileNewProcedureField { get; private set; } = new Word("Procedure"); 
        public Word FileNewTestField { get; private set; } = new Word("Test"); 
        public Word FileNewDatabaseField { get; private set; } = new Word("Database"); 
        public Word FileNewTableField { get; private set; } = new Word("Table"); 
        public Word FileNewSequenceField { get; private set; } = new Word("Sequence"); 
        public Word FileNewViewField { get; private set; } = new Word("View"); 
        public Word FileNewTriggerField { get; private set; } = new Word("Trigger"); 

        public Word FileOpenField { get; private set; } = new Word("Open"); 
        public Word FileOpenSqlFileField { get; private set; } = new Word("SQL file"); 
        public Word FileOpenFunctionField { get; private set; } = new Word("Function"); 
        public Word FileOpenProcedureField { get; private set; } = new Word("Procedure"); 
        public Word FileOpenTestField { get; private set; } = new Word("Test"); 
        public Word FileOpenDatabaseField { get; private set; } = new Word("Database"); 
        public Word FileOpenTableField { get; private set; } = new Word("Table"); 
        public Word FileOpenSequenceField { get; private set; } = new Word("Sequence"); 
        public Word FileOpenViewField { get; private set; } = new Word("View"); 
        public Word FileOpenTriggerField { get; private set; } = new Word("Trigger"); 

        public Word FileReopenField { get; private set; } = new Word("Reopen"); 
        public Word FileSaveField { get; private set; } = new Word("Save"); 
        public Word FileSaveAllField { get; private set; } = new Word("Save All"); 
        public Word FileCloseField { get; private set; } = new Word("Close"); 
        public Word FileCloseAllField { get; private set; } = new Word("Close All"); 
        public Word FileExitField { get; private set; } = new Word("Exit");
        #endregion  // File fields

        #region Edit fields
        public Word EditField { get; private set; } = new Word("Edit"); 
        public Word EditUndoField { get; private set; } = new Word("Undo"); 
        public Word EditRedoField { get; private set; } = new Word("Redo"); 
        public Word EditSettingsField { get; private set; } = new Word("Settings"); 
        #endregion  // Edit fields

        #region Pages fields
        public Word PagesField { get; private set; } = new Word("Pages"); 
        public Word PagesSqlQueryField { get; private set; } = new Word("SQL query"); 
        public Word PagesCommandLineField { get; private set; } = new Word("Command line"); 
        public Word PagesFunctionsField { get; private set; } = new Word("Functions"); 
        public Word PagesProceduresField { get; private set; } = new Word("Procedures"); 
        public Word PagesTestsField { get; private set; } = new Word("Tests"); 
        public Word PagesDatabasesField { get; private set; } = new Word("Databases"); 
        public Word PagesTablesField { get; private set; } = new Word("Tables"); 
        #endregion  // Pages fields

        #region Tools fields
        public Word ToolsField { get; private set; } = new Word("Tools"); 
        public Word ToolsOptionsField { get; private set; } = new Word("Options"); 
        public Word ToolsToolbarsField { get; private set; } = new Word("Toolbars"); 
        public Word ToolsConnectionsField { get; private set; } = new Word("Connections"); 
        #endregion  // Tools fields

        #region Help fields
        public Word HelpField { get; private set; } = new Word("Help"); 
        public Word HelpDocsField { get; private set; } = new Word("Common SQL docs"); 
        public Word HelpUserGuideField { get; private set; } = new Word("User guide"); 
        public Word HelpGithubField { get; private set; } = new Word("GitHub repository"); 
        public Word HelpReportField { get; private set; } = new Word("Report"); 
        #endregion  // Help fields

        #region File methods 
        public void SetFileField(System.String value)
        {
            FileField.SetTranslation(value); 
        }

        public void SetFileNewField(System.String value)
        {
            FileNewField.SetTranslation(value); 
        }

        public void SetFileNewSqlFileField(System.String value)
        {
            FileNewSqlFileField.SetTranslation(value); 
        }

        public void SetFileNewFunctionField(System.String value)
        {
            FileNewFunctionField.SetTranslation(value); 
        }

        public void SetFileNewProcedureField(System.String value)
        {
            FileNewProcedureField.SetTranslation(value); 
        }

        public void SetFileNewTestField(System.String value)
        {
            FileNewTestField.SetTranslation(value); 
        }

        public void SetFileNewDatabaseField(System.String value)
        {
            FileNewDatabaseField.SetTranslation(value); 
        }

        public void SetFileNewTableField(System.String value)
        {
            FileNewTableField.SetTranslation(value); 
        }

        public void SetFileNewSequenceField(System.String value)
        {
            FileNewSequenceField.SetTranslation(value); 
        }

        public void SetFileNewViewField(System.String value)
        {
            FileNewViewField.SetTranslation(value); 
        }

        public void SetFileNewTriggerField(System.String value)
        {
            FileNewTriggerField.SetTranslation(value); 
        }

        public void SetFileOpenField(System.String value)
        {
            FileOpenField.SetTranslation(value); 
        }

        public void SetFileOpenSqlFileField(System.String value)
        {
            FileOpenSqlFileField.SetTranslation(value); 
        }

        public void SetFileOpenFunctionField(System.String value)
        {
            FileOpenFunctionField.SetTranslation(value); 
        }

        public void SetFileOpenProcedureField(System.String value)
        {
            FileOpenProcedureField.SetTranslation(value); 
        }

        public void SetFileOpenTestField(System.String value)
        {
            FileOpenTestField.SetTranslation(value); 
        }

        public void SetFileOpenDatabaseField(System.String value)
        {
            FileOpenDatabaseField.SetTranslation(value); 
        }

        public void SetFileOpenTableField(System.String value)
        {
            FileOpenTableField.SetTranslation(value); 
        }

        public void SetFileOpenSequenceField(System.String value)
        {
            FileOpenSequenceField.SetTranslation(value); 
        }

        public void SetFileOpenViewField(System.String value)
        {
            FileOpenViewField.SetTranslation(value); 
        }

        public void SetFileOpenTriggerField(System.String value)
        {
            FileOpenTriggerField.SetTranslation(value); 
        }

        public void SetFileReopenField(System.String value)
        {
            FileReopenField.SetTranslation(value); 
        }

        public void SetFileSaveField(System.String value)
        {
            FileSaveField.SetTranslation(value); 
        }

        public void SetFileSaveAllField(System.String value)
        {
            FileSaveAllField.SetTranslation(value); 
        }

        public void SetFileCloseField(System.String value)
        {
            FileCloseField.SetTranslation(value); 
        }

        public void SetFileCloseAllField(System.String value)
        {
            FileCloseAllField.SetTranslation(value); 
        }

        public void SetFileExitField(System.String value)
        {
            FileExitField.SetTranslation(value); 
        }

        #endregion  // File methods 

        #region Edit methods
        public void SetEditField(System.String value)
        {
            EditField.SetTranslation(value); 
        }
        public void SetEditUndoField(System.String value)
        {
            EditUndoField.SetTranslation(value); 
        }
        public void SetEditRedoField(System.String value)
        {
            EditRedoField.SetTranslation(value); 
        }
        public void SetEditSettingsField(System.String value)
        {
            EditSettingsField.SetTranslation(value); 
        }
        #endregion  // Edit methods        

        #region Pages methods
        public void SetPagesField(System.String value)
        {
            PagesField.SetTranslation(value); 
        }
        public void SetPagesSqlQueryField(System.String value)
        {
            PagesSqlQueryField.SetTranslation(value); 
        }
        public void SetPagesCommandLineField(System.String value)
        {
            PagesCommandLineField.SetTranslation(value); 
        }
        public void SetPagesFunctionsField(System.String value)
        {
            PagesFunctionsField.SetTranslation(value); 
        }
        public void SetPagesProceduresField(System.String value)
        {
            PagesProceduresField.SetTranslation(value); 
        }
        public void SetPagesTestsField(System.String value)
        {
            PagesTestsField.SetTranslation(value); 
        }
        public void SetPagesDatabasesField(System.String value)
        {
            PagesDatabasesField.SetTranslation(value); 
        }
        public void SetPagesTablesField(System.String value)
        {
            PagesTablesField.SetTranslation(value); 
        }
        #endregion  // Pages methods

        #region Tools methods
        public void SetToolsField(System.String value)
        {
            ToolsField.SetTranslation(value); 
        }
        public void SetToolsOptionsField(System.String value)
        {
            ToolsOptionsField.SetTranslation(value); 
        }
        public void SetToolsToolbarsField(System.String value)
        {
            ToolsToolbarsField.SetTranslation(value); 
        }
        public void SetToolsConnectionsField(System.String value)
        {
            ToolsConnectionsField.SetTranslation(value); 
        }
        #endregion  // Tools methods

        #region Help methods
        public void SetHelpField(System.String value)
        {
            HelpField.SetTranslation(value); 
        }
        public void SetHelpDocsField(System.String value)
        {
            HelpDocsField.SetTranslation(value); 
        }
        public void SetHelpUserGuideField(System.String value)
        {
            HelpUserGuideField.SetTranslation(value); 
        }
        public void SetHelpGithubField(System.String value)
        {
            HelpGithubField.SetTranslation(value); 
        }
        public void SetHelpReportField(System.String value)
        {
            HelpReportField.SetTranslation(value); 
        }
        #endregion  // Help methods        
    }
}