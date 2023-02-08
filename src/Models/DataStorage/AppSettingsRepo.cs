using SqlViewer.Helpers; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DataStorage
{
    /// <summary>
    /// App settings at the current moment 
    /// </summary>
    public class AppSettingsRepo
    {
        #region Properties
        public ConfigSettings ConfigSettings { get; private set; }

        public LanguageEnum Language { get; private set; }
        public AutoSaveEnum AutoSave { get; private set; }
        public FontSizeEnum FontSize { get; private set; }
        public FontFamilyEnum FontFamily { get; private set; }
        public TabSizeEnum TabSize { get; private set; }
        public WordWrapEnum WordWrap { get; private set; }

        public RdbmsEnum DefaultRdbms { get; private set; }
        public RdbmsEnum ActiveRdbms { get; private set; }

        public string DbHost { get; private set; }
        public string DbPort { get; private set; }
        public string DbName { get; private set; }
        public string DbSchema { get; private set; }
        public string DbUsername { get; private set; }
        public string DbPassword { get; private set; }
        #endregion  // Properties

        #region Public methods 
        public void SetConfigSettings(ConfigSettings configSettings)
        {
            if (configSettings == null) throw new System.Exception("Parameter 'configSettings' could not be null"); 
            ConfigSettings = configSettings; 
        }

        public void SetLanguage(string language) 
        {
            if (string.IsNullOrEmpty(language)) throw new System.Exception("Parameter 'language' could not be null or empty"); 
            Language = RepoHelper.EnumEncoder.GetLanguageEnum(language); 
        }
        public void SetAutoSave(string autoSave) 
        {
            if (string.IsNullOrEmpty(autoSave)) throw new System.Exception("Parameter 'autoSave' could not be null or empty"); 
            AutoSave = RepoHelper.EnumEncoder.GetAutoSaveEnum(autoSave); 
        }
        public void SetFontSize(int fontSize) 
        {
            FontSize = RepoHelper.EnumEncoder.GetFontSizeEnum(fontSize.ToString()); 
        }
        public void SetFontFamily(string fontFamily) 
        {
            if (string.IsNullOrEmpty(fontFamily)) throw new System.Exception("Parameter 'fontFamily' could not be null or empty"); 
            FontFamily = RepoHelper.EnumEncoder.GetFontFamilyEnum(fontFamily); 
        }
        public void SetTabSize(int tabSize) 
        {
            TabSize = RepoHelper.EnumEncoder.GetTabSizeEnum(tabSize.ToString()); 
        }
        public void SetWordWrap(string wordWrap) 
        {
            if (string.IsNullOrEmpty(wordWrap)) throw new System.Exception("Parameter 'wordWrap' could not be null or empty"); 
            WordWrap = RepoHelper.EnumEncoder.GetWordWrapEnum(wordWrap); 
        }

        public void SetDefaultRdbms(string defaultRdbms) 
        {
            if (string.IsNullOrEmpty(defaultRdbms)) throw new System.Exception("Parameter 'defaultRdbms' could not be null or empty"); 
            DefaultRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(defaultRdbms); 
        }
        public void SetActiveRdbms(string activeRdbms) 
        {
            if (string.IsNullOrEmpty(activeRdbms)) throw new System.Exception("Parameter 'activeRdbms' could not be null or empty"); 
            ActiveRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(activeRdbms); 
        }

        public void SetDbHost(string dbHost) 
        {
            DbHost = dbHost; 
        }
        public void SetDbName(string dbName) 
        {
            DbName = dbName; 
        }
        public void SetDbPort(string dbPort) 
        {
            DbPort = dbPort; 
        }
        public void SetDbSchema(string dbSchema) 
        {
            DbSchema = dbSchema; 
        }
        public void SetDbUsername(string dbUsername) 
        {
            DbUsername = dbUsername; 
        }
        public void SetDbPassword(string dbPassword) 
        {
            DbPassword = dbPassword; 
        }
        #endregion  // Public methods 
    }
}
