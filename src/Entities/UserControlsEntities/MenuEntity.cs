using SqlViewer.Entities.Language;

namespace SqlViewer.Entities.UserControlsEntities
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuEntity 
    {
        #region File fields
        /// <summary>
        /// 
        /// </summary>
        public Word FileField { get; private set; } = new Word("File"); 

        /// <summary>
        /// 
        /// </summary>
        public Word FileNewField { get; private set; } = new Word("New"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewSqlFileField { get; private set; } = new Word("SQL file"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewFunctionField { get; private set; } = new Word("Function"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewProcedureField { get; private set; } = new Word("Procedure"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewTestField { get; private set; } = new Word("Test"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewDatabaseField { get; private set; } = new Word("Database"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewTableField { get; private set; } = new Word("Table"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewSequenceField { get; private set; } = new Word("Sequence"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewViewField { get; private set; } = new Word("View"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileNewTriggerField { get; private set; } = new Word("Trigger"); 

        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenField { get; private set; } = new Word("Open"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenSqlFileField { get; private set; } = new Word("SQL file"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenFunctionField { get; private set; } = new Word("Function"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenProcedureField { get; private set; } = new Word("Procedure"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenTestField { get; private set; } = new Word("Test"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenDatabaseField { get; private set; } = new Word("Database"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenTableField { get; private set; } = new Word("Table"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenSequenceField { get; private set; } = new Word("Sequence"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenViewField { get; private set; } = new Word("View"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileOpenTriggerField { get; private set; } = new Word("Trigger"); 

        /// <summary>
        /// 
        /// </summary>
        public Word FileReopenField { get; private set; } = new Word("Reopen"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileSaveField { get; private set; } = new Word("Save"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileSaveAllField { get; private set; } = new Word("Save All"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileCloseField { get; private set; } = new Word("Close"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileCloseAllField { get; private set; } = new Word("Close All"); 
        /// <summary>
        /// 
        /// </summary>
        public Word FileExitField { get; private set; } = new Word("Exit");
        #endregion  // File fields

        #region Edit fields
        /// <summary>
        /// 
        /// </summary>
        public Word EditField { get; private set; } = new Word("Edit"); 
        /// <summary>
        /// 
        /// </summary>
        public Word EditUndoField { get; private set; } = new Word("Undo"); 
        /// <summary>
        /// 
        /// </summary>
        public Word EditRedoField { get; private set; } = new Word("Redo"); 
        /// <summary>
        /// 
        /// </summary>
        public Word EditSettingsField { get; private set; } = new Word("Settings"); 
        #endregion  // Edit fields

        #region Pages fields
        /// <summary>
        /// 
        /// </summary>
        public Word PagesField { get; private set; } = new Word("Pages"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesSqlQueryField { get; private set; } = new Word("SQL query"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesCommandLineField { get; private set; } = new Word("Command line"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesFunctionsField { get; private set; } = new Word("Functions"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesProceduresField { get; private set; } = new Word("Procedures"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesTestsField { get; private set; } = new Word("Tests"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesDatabasesField { get; private set; } = new Word("Databases"); 
        /// <summary>
        /// 
        /// </summary>
        public Word PagesTablesField { get; private set; } = new Word("Tables"); 
        #endregion  // Pages fields

        #region Tools fields
        /// <summary>
        /// 
        /// </summary>
        public Word ToolsField { get; private set; } = new Word("Tools"); 
        /// <summary>
        /// 
        /// </summary>
        public Word ToolsOptionsField { get; private set; } = new Word("Options"); 
        /// <summary>
        /// 
        /// </summary>
        public Word ToolsToolbarsField { get; private set; } = new Word("Toolbars"); 
        /// <summary>
        /// 
        /// </summary>
        public Word ToolsConnectionsField { get; private set; } = new Word("Connections"); 
        #endregion  // Tools fields

        #region Help fields
        /// <summary>
        /// 
        /// </summary>
        public Word HelpField { get; private set; } = new Word("Help"); 
        /// <summary>
        /// 
        /// </summary>
        public Word HelpDocsField { get; private set; } = new Word("Common SQL docs"); 
        /// <summary>
        /// 
        /// </summary>
        public Word HelpUserGuideField { get; private set; } = new Word("User guide"); 
        /// <summary>
        /// 
        /// </summary>
        public Word HelpGithubField { get; private set; } = new Word("GitHub repository"); 
        /// <summary>
        /// 
        /// </summary>
        public Word HelpReportField { get; private set; } = new Word("Report"); 
        #endregion  // Help fields

        #region File methods 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileField(string value) => FileField.SetTranslation(value); 
        
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewField(string value) => FileNewField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewSqlFileField(string value) => FileNewSqlFileField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewFunctionField(string value) => FileNewFunctionField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewProcedureField(string value) => FileNewProcedureField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewTestField(string value) => FileNewTestField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewDatabaseField(string value) => FileNewDatabaseField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewTableField(string value) => FileNewTableField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewSequenceField(string value) => FileNewSequenceField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewViewField(string value) => FileNewViewField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileNewTriggerField(string value) => FileNewTriggerField.SetTranslation(value); 
        
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenField(string value) => FileOpenField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenSqlFileField(string value) => FileOpenSqlFileField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenFunctionField(string value) => FileOpenFunctionField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenProcedureField(string value) => FileOpenProcedureField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenTestField(string value) => FileOpenTestField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenDatabaseField(string value) => FileOpenDatabaseField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenTableField(string value) => FileOpenTableField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenSequenceField(string value) => FileOpenSequenceField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenViewField(string value) => FileOpenViewField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileOpenTriggerField(string value) => FileOpenTriggerField.SetTranslation(value); 
        
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileReopenField(string value) => FileReopenField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileSaveField(string value) => FileSaveField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileSaveAllField(string value) => FileSaveAllField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileCloseField(string value) => FileCloseField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileCloseAllField(string value) => FileCloseAllField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateFileExitField(string value) => FileExitField.SetTranslation(value); 
        #endregion  // File methods 

        #region Edit methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEditField(string value) => EditField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEditUndoField(string value) => EditUndoField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEditRedoField(string value) => EditRedoField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateEditSettingsField(string value) => EditSettingsField.SetTranslation(value); 
        #endregion  // Edit methods        

        #region Pages methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesField(string value) => PagesField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesSqlQueryField(string value) => PagesSqlQueryField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesCommandLineField(string value) => PagesCommandLineField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesFunctionsField(string value) => PagesFunctionsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesProceduresField(string value) => PagesProceduresField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesTestsField(string value) => PagesTestsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesDatabasesField(string value) => PagesDatabasesField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslatePagesTablesField(string value) => PagesTablesField.SetTranslation(value); 
        #endregion  // Pages methods

        #region Tools methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateToolsField(string value) => ToolsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateToolsOptionsField(string value) => ToolsOptionsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateToolsToolbarsField(string value) => ToolsToolbarsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateToolsConnectionsField(string value) => ToolsConnectionsField.SetTranslation(value); 
        #endregion  // Tools methods

        #region Help methods
        /// <summary>
        /// 
        /// </summary>
        public void TranslateHelpField(string value) => HelpField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateHelpDocsField(string value) => HelpDocsField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateHelpUserGuideField(string value) => HelpUserGuideField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateHelpGithubField(string value) => HelpGithubField.SetTranslation(value); 
        /// <summary>
        /// 
        /// </summary>
        public void TranslateHelpReportField(string value) => HelpReportField.SetTranslation(value); 
        #endregion  // Help methods        
    }
}