using System.Data;
using SqlViewer.Entities.Language;
using SqlViewer.Entities.ViewsEntities;
using SqlViewer.Entities.UserControlsEntities;
using SqlViewer.Entities.PagesEntities;
using LanguageEnum = SqlViewer.Enums.Common.Language;

namespace SqlViewer.Utils.Language;

public class Translator : BaseTranslator
{
    public LanguageEnum LanguageEnum { get; private set; }

    public LoginEntity LoginEntity { get; private set; } = new LoginEntity();
    public MenuEntity MenuEntity { get; private set; } = new MenuEntity();
    public SettingsEntity SettingsEntity { get; private set; } = new SettingsEntity();
    public SqlPageEntity SqlPageEntity { get; private set; } = new SqlPageEntity();
    public TablesPageEntity TablesPageEntity { get; private set; } = new TablesPageEntity();
    public ConnectionEntity ConnectionEntity { get; private set; } = new ConnectionEntity();

    public void SetLanguageEnum(LanguageEnum language)
    {
        LanguageEnum = language;
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        sql = string.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
        DataTable dt = Translate(sql);

        Word languageWord = new(LanguageEnum.ToString());
        SettingsEntity.SetChosenLanguageField(TranslateSingleWord(dt, LanguageEnum.ToString(), languageWord.English));
    }

    public void TranslateLogin()
    {
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        sql = string.Format(sql, LanguageEnum.ToString(), "LOGIN");
        DataTable dt = Translate(sql);

        LoginEntity.SetActiveRdbmsField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.ActiveRdbmsField.English));
        LoginEntity.SetDatabaseField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.DatabaseField.English));
        LoginEntity.SetSchemaField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.SchemaField.English));
        LoginEntity.SetUsernameField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.UsernameField.English));
        LoginEntity.SetPasswordField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.PasswordField.English));
            
        LoginEntity.SetLogInField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.LogInField.English));
        LoginEntity.SetCancelField(TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.CancelField.English));
    }

    public void TranslateMenu()
    {
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        sql = string.Format(sql, LanguageEnum.ToString(), "MENU");
        DataTable dt = Translate(sql);

        #region Translate File
        MenuEntity.SetFileField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileField.English));
            
        MenuEntity.SetFileNewField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewField.English));
        MenuEntity.SetFileNewSqlFileField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSqlFileField.English));
        MenuEntity.SetFileNewFunctionField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewFunctionField.English));
        MenuEntity.SetFileNewProcedureField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewProcedureField.English));
        MenuEntity.SetFileNewTestField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTestField.English));
        MenuEntity.SetFileNewDatabaseField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewDatabaseField.English));
        MenuEntity.SetFileNewTableField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTableField.English));
        MenuEntity.SetFileNewSequenceField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSequenceField.English));
        MenuEntity.SetFileNewViewField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewViewField.English));
        MenuEntity.SetFileNewTriggerField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTriggerField.English));
                            
        MenuEntity.SetFileOpenField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenField.English));
        MenuEntity.SetFileOpenSqlFileField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSqlFileField.English));
        MenuEntity.SetFileOpenFunctionField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenFunctionField.English));
        MenuEntity.SetFileOpenProcedureField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenProcedureField.English));
        MenuEntity.SetFileOpenTestField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTestField.English));
        MenuEntity.SetFileOpenDatabaseField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenDatabaseField.English));
        MenuEntity.SetFileOpenTableField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTableField.English));
        MenuEntity.SetFileOpenSequenceField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSequenceField.English));
        MenuEntity.SetFileOpenViewField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenViewField.English));
        MenuEntity.SetFileOpenTriggerField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTriggerField.English));

        MenuEntity.SetFileReopenField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileReopenField.English));
        MenuEntity.SetFileSaveField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveField.English));
        MenuEntity.SetFileSaveAllField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveAllField.English));
        MenuEntity.SetFileCloseField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseField.English));
        MenuEntity.SetFileCloseAllField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseAllField.English));
        MenuEntity.SetFileExitField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileExitField.English));
        #endregion  // Translate File

        #region Translate Edit 
        MenuEntity.SetEditField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditField.English));
        MenuEntity.SetEditUndoField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditUndoField.English));
        MenuEntity.SetEditRedoField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditRedoField.English));
        MenuEntity.SetEditSettingsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditSettingsField.English));
        #endregion  // Translate Edit

        #region Translate Pages
        MenuEntity.SetPagesField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesField.English));
        MenuEntity.SetPagesSqlQueryField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesSqlQueryField.English));
        MenuEntity.SetPagesCommandLineField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesCommandLineField.English));
        MenuEntity.SetPagesFunctionsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesFunctionsField.English));
        MenuEntity.SetPagesProceduresField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesProceduresField.English));
        MenuEntity.SetPagesTestsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTestsField.English));
        MenuEntity.SetPagesDatabasesField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesDatabasesField.English));
        MenuEntity.SetPagesTablesField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTablesField.English));
        #endregion  // Translate Pages

        #region Translate Tools
        MenuEntity.SetToolsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsField.English));
        MenuEntity.SetToolsOptionsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsOptionsField.English));
        MenuEntity.SetToolsToolbarsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsToolbarsField.English));
        MenuEntity.SetToolsConnectionsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsConnectionsField.English));
        #endregion  // Translate Tools

        #region Translate Help
        MenuEntity.SetHelpField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpField.English));
        MenuEntity.SetHelpDocsField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpDocsField.English));
        MenuEntity.SetHelpUserGuideField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpUserGuideField.English));
        MenuEntity.SetHelpGithubField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpGithubField.English));
        MenuEntity.SetHelpReportField(TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpReportField.English));
        #endregion  // Translate Help
    }

    public void TranslateSettings()
    {
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        #region Translate Settings  
        sql = string.Format(sql, LanguageEnum.ToString(), "SETTINGS");
        DataTable dt = Translate(sql);

        SettingsEntity.SetEditorField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EditorField.English));
        SettingsEntity.SetLanguageField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.LanguageField.English));
        SettingsEntity.SetAutoSaveField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.AutoSaveField.English));
        SettingsEntity.SetFontSizeField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontSizeField.English));
        SettingsEntity.SetFontFamilyField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontFamilyField.English));
        SettingsEntity.SetTabSizeField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.TabSizeField.English));
        SettingsEntity.SetWordWrapField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.WordWrapField.English));

        SettingsEntity.SetDbField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DbField.English));
        SettingsEntity.SetDefaultRdbmsField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DefaultRdbmsField.English));
        SettingsEntity.SetActiveRdbmsField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ActiveRdbmsField.English));
        SettingsEntity.SetDatabaseField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English));
        SettingsEntity.SetSchemaField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SchemaField.English));
        SettingsEntity.SetUsernameField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.UsernameField.English));
        SettingsEntity.SetPasswordField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.PasswordField.English));
            
        SettingsEntity.SetRecoverField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.RecoverField.English));
        SettingsEntity.SetSaveField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SaveField.English));
        SettingsEntity.SetCancelField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.CancelField.English));
        #endregion  // Translate Settings  

        #region Translate Common 
        sql = string.Format(sql, LanguageEnum.ToString(), "COMMON");
        dt = Translate(sql);

        SettingsEntity.SetDatabaseField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English));
        SettingsEntity.SetEnabledField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EnabledField.English));
        SettingsEntity.SetDisabledField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DisabledField.English));
        #endregion  // Translate Common 

        #region Translate Language 
        sql = string.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
        dt = Translate(sql);

        SettingsEntity.SetChosenLanguageField(TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ChosenLanguageField.English));
        #endregion  // Translate Language
    }

    public void TranslatePages()
    {
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        sql = string.Format(sql, LanguageEnum.ToString(), "PAGES");
        DataTable dt = Translate(sql);

        #region Translate SQL page
        SqlPageEntity.SetPathField(TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.PathField.English));
        SqlPageEntity.SetExecuteField(TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.ExecuteField.English));
        #endregion  // Translate SQL page 

        #region Translate Tables page
        TablesPageEntity.SetPathField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.PathField.English));
        TablesPageEntity.SetTablesField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TablesField.English));
        TablesPageEntity.SetGeneralInfoField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.GeneralInfoField.English));
        TablesPageEntity.SetColumnsField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ColumnsField.English));
        TablesPageEntity.SetForeignKeysField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ForeignKeysField.English));
        TablesPageEntity.SetTriggersField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TriggersField.English));
        TablesPageEntity.SetDataField(TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.DataField.English));
        #endregion  // Translate Tables page
    }

    public void TranslateConnection()
    {
        string sql = @"
SELECT t.english, t.{0}
FROM translation t
WHERE UPPER(t.context) LIKE UPPER('{1}');";
        sql = string.Format(sql, LanguageEnum.ToString(), "CONNECTION");
        DataTable dt = Translate(sql);

        ConnectionEntity.SetActiveRdbmsField(TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.ActiveRdbmsField.English));
        ConnectionEntity.SetExecuteField(TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.ExecuteField.English));
        ConnectionEntity.SetTransferField(TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.TransferField.English));
    }
}