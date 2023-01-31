using System.Data;  
using SqlViewer.ViewModels; 
using SqlViewer.Entities.Language;
using SqlViewer.Entities.ViewsEntities; 
using SqlViewer.Entities.UserControlsEntities; 
using SqlViewer.Entities.PagesEntities;
using LanguageEnum = SqlViewer.Enums.Common.Language; 

namespace SqlViewer.Utils.Language
{
    /// <summary>
    /// 
    /// </summary>
    public class Translator : BaseTranslator, ICommonMsgTranslator, ISettingsTranslator
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LanguageEnum LanguageEnum { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public LoginEntity LoginEntity { get; private set; } = new LoginEntity(); 
        /// <summary>
        /// 
        /// </summary>
        public MenuEntity MenuEntity { get; private set; } = new MenuEntity(); 
        /// <summary>
        /// 
        /// </summary>
        public SettingsEntity SettingsEntity { get; private set; } = new SettingsEntity(); 
        /// <summary>
        /// 
        /// </summary>
        public SqlPageEntity SqlPageEntity { get; private set; } = new SqlPageEntity(); 
        /// <summary>
        /// 
        /// </summary>
        public TablesPageEntity TablesPageEntity { get; private set; } = new TablesPageEntity(); 
        /// <summary>
        /// 
        /// </summary>
        public ConnectionEntity ConnectionEntity { get; private set; } = new ConnectionEntity(); 
        #endregion  // Properties

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public Translator(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }
        #endregion  // Constructors

        #region Public methods
        /// <summary>
        /// 
        /// </summary>
        public void SetLanguageEnum(LanguageEnum language)
        {
            LanguageEnum = language; 
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
                DataTable dt = base.Translate(sql); 

                var languageWord = new Word(LanguageEnum.ToString()); 
                SettingsEntity.TranslateChosenLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), languageWord.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TranslateLogin()
        {
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "LOGIN");
                DataTable dt = base.Translate(sql); 

                LoginEntity.TranslateActiveRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.ActiveRdbmsField.English)); 
                LoginEntity.TranslateDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.DatabaseField.English)); 
                LoginEntity.TranslateSchemaField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.SchemaField.English)); 
                LoginEntity.TranslateUsernameField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.UsernameField.English)); 
                LoginEntity.TranslatePasswordField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.PasswordField.English)); 
                
                LoginEntity.TranslateLogInField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.LogInField.English)); 
                LoginEntity.TranslateCancelField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.CancelField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TranslateMenu()
        {
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "MENU");
                DataTable dt = base.Translate(sql); 
                //
                // Translate File
                //
                MenuEntity.TranslateFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileField.English)); 
                
                MenuEntity.TranslateFileNewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewField.English)); 
                MenuEntity.TranslateFileNewSqlFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSqlFileField.English)); 
                MenuEntity.TranslateFileNewFunctionField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewFunctionField.English)); 
                MenuEntity.TranslateFileNewProcedureField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewProcedureField.English)); 
                MenuEntity.TranslateFileNewTestField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTestField.English)); 
                MenuEntity.TranslateFileNewDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewDatabaseField.English)); 
                MenuEntity.TranslateFileNewTableField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTableField.English)); 
                MenuEntity.TranslateFileNewSequenceField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSequenceField.English)); 
                MenuEntity.TranslateFileNewViewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewViewField.English)); 
                MenuEntity.TranslateFileNewTriggerField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTriggerField.English)); 
                                
                MenuEntity.TranslateFileOpenField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenField.English)); 
                MenuEntity.TranslateFileOpenSqlFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSqlFileField.English));
                MenuEntity.TranslateFileOpenFunctionField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenFunctionField.English));
                MenuEntity.TranslateFileOpenProcedureField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenProcedureField.English));
                MenuEntity.TranslateFileOpenTestField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTestField.English));
                MenuEntity.TranslateFileOpenDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenDatabaseField.English));
                MenuEntity.TranslateFileOpenTableField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTableField.English));
                MenuEntity.TranslateFileOpenSequenceField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSequenceField.English));
                MenuEntity.TranslateFileOpenViewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenViewField.English));
                MenuEntity.TranslateFileOpenTriggerField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTriggerField.English));

                MenuEntity.TranslateFileReopenField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileReopenField.English));
                MenuEntity.TranslateFileSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveField.English));
                MenuEntity.TranslateFileSaveAllField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveAllField.English));
                MenuEntity.TranslateFileCloseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseField.English));
                MenuEntity.TranslateFileCloseAllField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseAllField.English));
                MenuEntity.TranslateFileExitField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileExitField.English));
                //
                // Translate Edit
                //
                MenuEntity.TranslateEditField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditField.English)); 
                MenuEntity.TranslateEditUndoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditUndoField.English)); 
                MenuEntity.TranslateEditRedoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditRedoField.English)); 
                MenuEntity.TranslateEditSettingsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditSettingsField.English)); 
                //
                // Translate Pages
                //
                MenuEntity.TranslatePagesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesField.English)); 
                MenuEntity.TranslatePagesSqlQueryField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesSqlQueryField.English)); 
                MenuEntity.TranslatePagesCommandLineField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesCommandLineField.English)); 
                MenuEntity.TranslatePagesFunctionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesFunctionsField.English)); 
                MenuEntity.TranslatePagesProceduresField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesProceduresField.English)); 
                MenuEntity.TranslatePagesTestsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTestsField.English)); 
                MenuEntity.TranslatePagesDatabasesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesDatabasesField.English)); 
                MenuEntity.TranslatePagesTablesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTablesField.English)); 
                //
                // Translate Tools
                //
                MenuEntity.TranslateToolsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsField.English)); 
                MenuEntity.TranslateToolsOptionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsOptionsField.English)); 
                MenuEntity.TranslateToolsToolbarsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsToolbarsField.English)); 
                MenuEntity.TranslateToolsConnectionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsConnectionsField.English)); 
                //
                // Translate Help
                //
                MenuEntity.TranslateHelpField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpField.English)); 
                MenuEntity.TranslateHelpDocsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpDocsField.English)); 
                MenuEntity.TranslateHelpUserGuideField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpUserGuideField.English)); 
                MenuEntity.TranslateHelpGithubField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpGithubField.English)); 
                MenuEntity.TranslateHelpReportField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpReportField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TranslateSettings()
        {
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                //
                // Translate Settings 
                //
                sql = string.Format(sql, LanguageEnum.ToString(), "SETTINGS");
                DataTable dt = base.Translate(sql); 

                SettingsEntity.TranslateEditorField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EditorField.English)); 
                SettingsEntity.TranslateLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.LanguageField.English)); 
                SettingsEntity.TranslateAutoSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.AutoSaveField.English)); 
                SettingsEntity.TranslateFontSizeField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontSizeField.English)); 
                SettingsEntity.TranslateFontFamilyField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontFamilyField.English)); 
                SettingsEntity.TranslateTabSizeField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.TabSizeField.English)); 
                SettingsEntity.TranslateWordWrapField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.WordWrapField.English)); 

                SettingsEntity.TranslateDbField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DbField.English)); 
                SettingsEntity.TranslateDefaultRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DefaultRdbmsField.English)); 
                SettingsEntity.TranslateActiveRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ActiveRdbmsField.English)); 
                SettingsEntity.TranslateDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English)); 
                SettingsEntity.TranslateSchemaField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SchemaField.English)); 
                SettingsEntity.TranslateUsernameField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.UsernameField.English)); 
                SettingsEntity.TranslatePasswordField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.PasswordField.English)); 
                
                SettingsEntity.TranslateRecoverField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.RecoverField.English)); 
                SettingsEntity.TranslateSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SaveField.English)); 
                SettingsEntity.TranslateCancelField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.CancelField.English)); 
                //
                // Translate Common 
                //
                sql = string.Format(sql, LanguageEnum.ToString(), "COMMON");
                dt = base.Translate(sql); 

                SettingsEntity.TranslateDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English)); 
                SettingsEntity.TranslateEnabledField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EnabledField.English)); 
                SettingsEntity.TranslateDisabledField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DisabledField.English)); 
                //
                // Translate Language 
                // 
                sql = string.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
                dt = base.Translate(sql); 

                SettingsEntity.TranslateChosenLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ChosenLanguageField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TranslatePages()
        {
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "PAGES");
                DataTable dt = base.Translate(sql); 
                //
                // Translate SQL page 
                // 
                SqlPageEntity.TranslatePathField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.PathField.English)); 
                SqlPageEntity.TranslateExecuteField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.ExecuteField.English)); 
                //
                // Translate Tables page 
                // 
                TablesPageEntity.TranslatePathField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.PathField.English)); 
                TablesPageEntity.TranslateTablesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TablesField.English)); 
                TablesPageEntity.TranslateGeneralInfoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.GeneralInfoField.English)); 
                TablesPageEntity.TranslateColumnsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ColumnsField.English)); 
                TablesPageEntity.TranslateForeignKeysField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ForeignKeysField.English)); 
                TablesPageEntity.TranslateTriggersField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TriggersField.English)); 
                TablesPageEntity.TranslateDataField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.DataField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TranslateConnection()
        {
            try
            {
                string sql = this.MainVM.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromTranslation.sql"); 
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "CONNECTION");
                DataTable dt = base.Translate(sql); 

                ConnectionEntity.TranslateActiveRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.ActiveRdbmsField.English)); 
                ConnectionEntity.TranslateExecuteField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.ExecuteField.English)); 
                ConnectionEntity.TranslateTransferField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), ConnectionEntity.TransferField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        #endregion  // Public methods
    }
}