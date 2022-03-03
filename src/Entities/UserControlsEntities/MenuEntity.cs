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
        public void SetFileField(string value)
        {
            FileField.SetTranslation(value); 
        }

        public void SetFileNewField(string value)
        {
            FileNewField.SetTranslation(value); 
        }

        public void SetFileNewSqlFileField(string value)
        {
            FileNewSqlFileField.SetTranslation(value); 
        }

        public void SetFileNewFunctionField(string value)
        {
            FileNewFunctionField.SetTranslation(value); 
        }

        public void SetFileNewProcedureField(string value)
        {
            FileNewProcedureField.SetTranslation(value); 
        }

        public void SetFileNewTestField(string value)
        {
            FileNewTestField.SetTranslation(value); 
        }

        public void SetFileNewDatabaseField(string value)
        {
            FileNewDatabaseField.SetTranslation(value); 
        }

        public void SetFileNewTableField(string value)
        {
            FileNewTableField.SetTranslation(value); 
        }

        public void SetFileNewSequenceField(string value)
        {
            FileNewSequenceField.SetTranslation(value); 
        }

        public void SetFileNewViewField(string value)
        {
            FileNewViewField.SetTranslation(value); 
        }

        public void SetFileNewTriggerField(string value)
        {
            FileNewTriggerField.SetTranslation(value); 
        }

        public void SetFileOpenField(string value)
        {
            FileOpenField.SetTranslation(value); 
        }

        public void SetFileOpenSqlFileField(string value)
        {
            FileOpenSqlFileField.SetTranslation(value); 
        }

        public void SetFileOpenFunctionField(string value)
        {
            FileOpenFunctionField.SetTranslation(value); 
        }

        public void SetFileOpenProcedureField(string value)
        {
            FileOpenProcedureField.SetTranslation(value); 
        }

        public void SetFileOpenTestField(string value)
        {
            FileOpenTestField.SetTranslation(value); 
        }

        public void SetFileOpenDatabaseField(string value)
        {
            FileOpenDatabaseField.SetTranslation(value); 
        }

        public void SetFileOpenTableField(string value)
        {
            FileOpenTableField.SetTranslation(value); 
        }

        public void SetFileOpenSequenceField(string value)
        {
            FileOpenSequenceField.SetTranslation(value); 
        }

        public void SetFileOpenViewField(string value)
        {
            FileOpenViewField.SetTranslation(value); 
        }

        public void SetFileOpenTriggerField(string value)
        {
            FileOpenTriggerField.SetTranslation(value); 
        }

        public void SetFileReopenField(string value)
        {
            FileReopenField.SetTranslation(value); 
        }

        public void SetFileSaveField(string value)
        {
            FileSaveField.SetTranslation(value); 
        }

        public void SetFileSaveAllField(string value)
        {
            FileSaveAllField.SetTranslation(value); 
        }

        public void SetFileCloseField(string value)
        {
            FileCloseField.SetTranslation(value); 
        }

        public void SetFileCloseAllField(string value)
        {
            FileCloseAllField.SetTranslation(value); 
        }

        public void SetFileExitField(string value)
        {
            FileExitField.SetTranslation(value); 
        }

        #endregion  // File methods 

        #region Edit methods
        public void SetEditField(string value)
        {
            EditField.SetTranslation(value); 
        }
        public void SetEditUndoField(string value)
        {
            EditUndoField.SetTranslation(value); 
        }
        public void SetEditRedoField(string value)
        {
            EditRedoField.SetTranslation(value); 
        }
        public void SetEditSettingsField(string value)
        {
            EditSettingsField.SetTranslation(value); 
        }
        #endregion  // Edit methods        

        #region Pages methods
        public void SetPagesField(string value)
        {
            PagesField.SetTranslation(value); 
        }
        public void SetPagesSqlQueryField(string value)
        {
            PagesSqlQueryField.SetTranslation(value); 
        }
        public void SetPagesCommandLineField(string value)
        {
            PagesCommandLineField.SetTranslation(value); 
        }
        public void SetPagesFunctionsField(string value)
        {
            PagesFunctionsField.SetTranslation(value); 
        }
        public void SetPagesProceduresField(string value)
        {
            PagesProceduresField.SetTranslation(value); 
        }
        public void SetPagesTestsField(string value)
        {
            PagesTestsField.SetTranslation(value); 
        }
        public void SetPagesDatabasesField(string value)
        {
            PagesDatabasesField.SetTranslation(value); 
        }
        public void SetPagesTablesField(string value)
        {
            PagesTablesField.SetTranslation(value); 
        }
        #endregion  // Pages methods

        #region Tools methods
        public void SetToolsField(string value)
        {
            ToolsField.SetTranslation(value); 
        }
        public void SetToolsOptionsField(string value)
        {
            ToolsOptionsField.SetTranslation(value); 
        }
        public void SetToolsToolbarsField(string value)
        {
            ToolsToolbarsField.SetTranslation(value); 
        }
        public void SetToolsConnectionsField(string value)
        {
            ToolsConnectionsField.SetTranslation(value); 
        }
        #endregion  // Tools methods

        #region Help methods
        public void SetHelpField(string value)
        {
            HelpField.SetTranslation(value); 
        }
        public void SetHelpDocsField(string value)
        {
            HelpDocsField.SetTranslation(value); 
        }
        public void SetHelpUserGuideField(string value)
        {
            HelpUserGuideField.SetTranslation(value); 
        }
        public void SetHelpGithubField(string value)
        {
            HelpGithubField.SetTranslation(value); 
        }
        public void SetHelpReportField(string value)
        {
            HelpReportField.SetTranslation(value); 
        }
        #endregion  // Help methods        
    }
}