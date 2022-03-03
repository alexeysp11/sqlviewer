using System.Data;  
using SqlViewer.ViewModels; 
using SqlViewer.Entities.ViewsEntities; 
using SqlViewer.Entities.UserControlsEntities; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 

namespace SqlViewer.Utils.Language
{
    public class Translator : BaseTranslator, ICommonMsgTranslator, ISettingsTranslator
    {
        private MainVM MainVM { get; set; }

        public LanguageEnum LanguageEnum { get; private set; }

        public MenuEntity MenuEntity { get; private set; } = new MenuEntity(); 

        public Translator(MainVM mainVM)
        {
            this.MainVM = mainVM;
        }

        public void TranslateMenu()
        {
            string sql = this.MainVM.GetSqlRequest("App/SelectFromTranslation.sql"); 
            try
            {
                if (sql == string.Empty)
                {
                    throw new System.Exception("Error while translation: SQL request should not be empty."); 
                }
                sql = string.Format(sql, LanguageEnum.ToString(), "MENU");
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

        public void SetLanguageEnum(LanguageEnum language)
        {
            LanguageEnum = language; 
        }
    }
}