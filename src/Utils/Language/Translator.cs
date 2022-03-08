using System.Data;  
using SqlViewer.ViewModels; 
using SqlViewer.Entities.Language;
using SqlViewer.Entities.ViewsEntities; 
using SqlViewer.Entities.UserControlsEntities; 
using SqlViewer.Entities.PagesEntities;
using LanguageEnum = SqlViewer.Enums.Common.Language; 

namespace SqlViewer.Utils.Language
{
    public class Translator : BaseTranslator, ICommonMsgTranslator, ISettingsTranslator
    {
        private MainVM MainVM { get; set; }

        public LanguageEnum LanguageEnum { get; private set; }

        public LoginEntity LoginEntity { get; private set; } = new LoginEntity(); 
        public MenuEntity MenuEntity { get; private set; } = new MenuEntity(); 
        public SettingsEntity SettingsEntity { get; private set; } = new SettingsEntity(); 
        public SqlPageEntity SqlPageEntity { get; private set; } = new SqlPageEntity(); 
        public TablesPageEntity TablesPageEntity { get; private set; } = new TablesPageEntity(); 

        public Translator(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }

        public void SetLanguageEnum(LanguageEnum language)
        {
            LanguageEnum = language; 

            System.String sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == System.String.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = System.String.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
                DataTable dt = base.Translate(sql); 

                var languageWord = new Word(LanguageEnum.ToString()); 
                SettingsEntity.SetChosenLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), languageWord.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void TranslateLogin()
        {
            System.String sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == System.String.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = System.String.Format(sql, LanguageEnum.ToString(), "LOGIN");
                DataTable dt = base.Translate(sql); 

                LoginEntity.SetActiveRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.ActiveRdbmsField.English)); 
                LoginEntity.SetDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.DatabaseField.English)); 
                LoginEntity.SetSchemaField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.SchemaField.English)); 
                LoginEntity.SetUsernameField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.UsernameField.English)); 
                LoginEntity.SetPasswordField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.PasswordField.English)); 
                
                LoginEntity.SetLogInField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.LogInField.English)); 
                LoginEntity.SetCancelField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), LoginEntity.CancelField.English)); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void TranslateMenu()
        {
            System.String sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == System.String.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = System.String.Format(sql, LanguageEnum.ToString(), "MENU");
                DataTable dt = base.Translate(sql); 

                #region Translate File
                MenuEntity.SetFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileField.English)); 
                
                MenuEntity.SetFileNewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewField.English)); 
                MenuEntity.SetFileNewSqlFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSqlFileField.English)); 
                MenuEntity.SetFileNewFunctionField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewFunctionField.English)); 
                MenuEntity.SetFileNewProcedureField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewProcedureField.English)); 
                MenuEntity.SetFileNewTestField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTestField.English)); 
                MenuEntity.SetFileNewDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewDatabaseField.English)); 
                MenuEntity.SetFileNewTableField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTableField.English)); 
                MenuEntity.SetFileNewSequenceField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewSequenceField.English)); 
                MenuEntity.SetFileNewViewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewViewField.English)); 
                MenuEntity.SetFileNewTriggerField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileNewTriggerField.English)); 
                                
                MenuEntity.SetFileOpenField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenField.English)); 
                MenuEntity.SetFileOpenSqlFileField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSqlFileField.English));
                MenuEntity.SetFileOpenFunctionField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenFunctionField.English));
                MenuEntity.SetFileOpenProcedureField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenProcedureField.English));
                MenuEntity.SetFileOpenTestField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTestField.English));
                MenuEntity.SetFileOpenDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenDatabaseField.English));
                MenuEntity.SetFileOpenTableField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTableField.English));
                MenuEntity.SetFileOpenSequenceField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenSequenceField.English));
                MenuEntity.SetFileOpenViewField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenViewField.English));
                MenuEntity.SetFileOpenTriggerField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileOpenTriggerField.English));

                MenuEntity.SetFileReopenField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileReopenField.English));
                MenuEntity.SetFileSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveField.English));
                MenuEntity.SetFileSaveAllField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileSaveAllField.English));
                MenuEntity.SetFileCloseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseField.English));
                MenuEntity.SetFileCloseAllField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileCloseAllField.English));
                MenuEntity.SetFileExitField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.FileExitField.English));
                #endregion  // Translate File

                #region Translate Edit 
                MenuEntity.SetEditField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditField.English)); 
                MenuEntity.SetEditUndoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditUndoField.English)); 
                MenuEntity.SetEditRedoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditRedoField.English)); 
                MenuEntity.SetEditSettingsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.EditSettingsField.English)); 
                #endregion  // Translate Edit

                #region Translate Pages
                MenuEntity.SetPagesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesField.English)); 
                MenuEntity.SetPagesSqlQueryField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesSqlQueryField.English)); 
                MenuEntity.SetPagesCommandLineField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesCommandLineField.English)); 
                MenuEntity.SetPagesFunctionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesFunctionsField.English)); 
                MenuEntity.SetPagesProceduresField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesProceduresField.English)); 
                MenuEntity.SetPagesTestsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTestsField.English)); 
                MenuEntity.SetPagesDatabasesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesDatabasesField.English)); 
                MenuEntity.SetPagesTablesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.PagesTablesField.English)); 
                #endregion  // Translate Pages

                #region Translate Tools
                MenuEntity.SetToolsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsField.English)); 
                MenuEntity.SetToolsOptionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsOptionsField.English)); 
                MenuEntity.SetToolsToolbarsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsToolbarsField.English)); 
                MenuEntity.SetToolsConnectionsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.ToolsConnectionsField.English)); 
                #endregion  // Translate Tools

                #region Translate Help
                MenuEntity.SetHelpField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpField.English)); 
                MenuEntity.SetHelpDocsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpDocsField.English)); 
                MenuEntity.SetHelpUserGuideField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpUserGuideField.English)); 
                MenuEntity.SetHelpGithubField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpGithubField.English)); 
                MenuEntity.SetHelpReportField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), MenuEntity.HelpReportField.English)); 
                #endregion  // Translate Help
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void TranslateSettings()
        {
            System.String sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == System.String.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                #region Translate Settings  
                sql = System.String.Format(sql, LanguageEnum.ToString(), "SETTINGS");
                DataTable dt = base.Translate(sql); 

                SettingsEntity.SetEditorField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EditorField.English)); 
                SettingsEntity.SetLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.LanguageField.English)); 
                SettingsEntity.SetAutoSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.AutoSaveField.English)); 
                SettingsEntity.SetFontSizeField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontSizeField.English)); 
                SettingsEntity.SetFontFamilyField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.FontFamilyField.English)); 
                SettingsEntity.SetTabSizeField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.TabSizeField.English)); 
                SettingsEntity.SetWordWrapField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.WordWrapField.English)); 

                SettingsEntity.SetDbField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DbField.English)); 
                SettingsEntity.SetDefaultRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DefaultRdbmsField.English)); 
                SettingsEntity.SetActiveRdbmsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ActiveRdbmsField.English)); 
                SettingsEntity.SetDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English)); 
                SettingsEntity.SetSchemaField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SchemaField.English)); 
                SettingsEntity.SetUsernameField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.UsernameField.English)); 
                SettingsEntity.SetPasswordField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.PasswordField.English)); 
                
                SettingsEntity.SetRecoverField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.RecoverField.English)); 
                SettingsEntity.SetSaveField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.SaveField.English)); 
                SettingsEntity.SetCancelField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.CancelField.English)); 
                #endregion  // Translate Settings  

                #region Translate Common 
                sql = System.String.Format(sql, LanguageEnum.ToString(), "COMMON");
                dt = base.Translate(sql); 

                SettingsEntity.SetDatabaseField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DatabaseField.English)); 
                SettingsEntity.SetEnabledField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.EnabledField.English)); 
                SettingsEntity.SetDisabledField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.DisabledField.English)); 
                #endregion  // Translate Common 

                #region Translate Language 
                sql = System.String.Format(sql, LanguageEnum.ToString(), "LANGUAGE");
                dt = base.Translate(sql); 

                SettingsEntity.SetChosenLanguageField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SettingsEntity.ChosenLanguageField.English)); 
                #endregion  // Translate Language 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void TranslatePages()
        {
            System.String sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == System.String.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = System.String.Format(sql, LanguageEnum.ToString(), "PAGES");
                DataTable dt = base.Translate(sql); 

                #region Translate SQL page
                SqlPageEntity.SetPathField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.PathField.English)); 
                SqlPageEntity.SetExecuteField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), SqlPageEntity.ExecuteField.English)); 
                #endregion  // Translate SQL page 

                #region Translate Tables page
                TablesPageEntity.SetPathField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.PathField.English)); 
                TablesPageEntity.SetTablesField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TablesField.English)); 
                TablesPageEntity.SetGeneralInfoField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.GeneralInfoField.English)); 
                TablesPageEntity.SetColumnsField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ColumnsField.English)); 
                TablesPageEntity.SetForeignKeysField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.ForeignKeysField.English)); 
                TablesPageEntity.SetTriggersField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.TriggersField.English)); 
                TablesPageEntity.SetDataField(base.TranslateSingleWord(dt, LanguageEnum.ToString(), TablesPageEntity.DataField.English)); 
                #endregion  // Translate Tables page 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}