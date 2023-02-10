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
        /// <summary>
        /// Settings from config file 
        /// </summary>
        public ConfigSettings ConfigSettings { get; private set; }

        /// <summary>
        /// Language of the application 
        /// </summary>
        public LanguageEnum Language { get; private set; }

        /// <summary>
        /// Parameter AutoSave for editor
        /// </summary>
        public AutoSaveEnum AutoSave { get; private set; }
        /// <summary>
        /// Parameter FontSize for editor
        /// </summary>
        public FontSizeEnum FontSize { get; private set; }
        /// <summary>
        /// Parameter FontFamily for editor
        /// </summary>
        public FontFamilyEnum FontFamily { get; private set; }
        /// <summary>
        /// Parameter TabSize for editor
        /// </summary>
        public TabSizeEnum TabSize { get; private set; }
        /// <summary>
        /// Parameter WordWrap for editor
        /// </summary>
        public WordWrapEnum WordWrap { get; private set; }

        /// <summary>
        /// Default RDBMS 
        /// </summary>
        public RdbmsEnum DefaultRdbms { get; private set; }
        /// <summary>
        /// Active RDBMS 
        /// </summary>
        public RdbmsEnum ActiveRdbms { get; private set; }

        /// <summary>
        /// Parameter Host for database connection 
        /// </summary>
        public string DbHost { get; private set; }
        /// <summary>
        /// Parameter Port for database connection 
        /// </summary>
        public string DbPort { get; private set; }
        /// <summary>
        /// Parameter Name for database connection 
        /// </summary>
        public string DbName { get; private set; }
        /// <summary>
        /// Parameter Schema for database connection 
        /// </summary>
        public string DbSchema { get; private set; }
        /// <summary>
        /// Parameter Username for database connection 
        /// </summary>
        public string DbUsername { get; private set; }
        /// <summary>
        /// Parameter Password for database connection 
        /// </summary>
        public string DbPassword { get; private set; }
        #endregion  // Properties

        #region Public methods 
        /// <summary>
        /// Sets settings from config file 
        /// </summary>
        public void SetConfigSettings(ConfigSettings configSettings)
        {
            if (configSettings == null) throw new System.Exception("Parameter 'configSettings' could not be null"); 
            ConfigSettings = configSettings; 
        }

        /// <summary>
        /// Sets language of the application 
        /// </summary>
        public void SetLanguage(string language) 
        {
            if (string.IsNullOrEmpty(language)) throw new System.Exception("Parameter 'language' could not be null or empty"); 
            Language = RepoHelper.EnumEncoder.GetLanguageEnum(language); 
        }

        /// <summary>
        /// Sets parameter AutoSave for editor
        /// </summary>
        public void SetAutoSave(string autoSave) 
        {
            if (string.IsNullOrEmpty(autoSave)) throw new System.Exception("Parameter 'autoSave' could not be null or empty"); 
            AutoSave = RepoHelper.EnumEncoder.GetAutoSaveEnum(autoSave); 
        }
        /// <summary>
        /// Sets parameter FontSize for editor
        /// </summary>
        public void SetFontSize(int fontSize) 
        {
            FontSize = RepoHelper.EnumEncoder.GetFontSizeEnum(fontSize.ToString()); 
        }
        /// <summary>
        /// Sets parameter FontFamily for editor
        /// </summary>
        public void SetFontFamily(string fontFamily) 
        {
            if (string.IsNullOrEmpty(fontFamily)) throw new System.Exception("Parameter 'fontFamily' could not be null or empty"); 
            FontFamily = RepoHelper.EnumEncoder.GetFontFamilyEnum(fontFamily); 
        }
        /// <summary>
        /// Sets parameter TabSize for editor
        /// </summary>
        public void SetTabSize(int tabSize) 
        {
            TabSize = RepoHelper.EnumEncoder.GetTabSizeEnum(tabSize.ToString()); 
        }
        /// <summary>
        /// Sets parameter WordWrap for editor
        /// </summary>
        public void SetWordWrap(string wordWrap) 
        {
            if (string.IsNullOrEmpty(wordWrap)) throw new System.Exception("Parameter 'wordWrap' could not be null or empty"); 
            WordWrap = RepoHelper.EnumEncoder.GetWordWrapEnum(wordWrap); 
        }

        /// <summary>
        /// Sets Default RDBMS 
        /// </summary>
        public void SetDefaultRdbms(string defaultRdbms) 
        {
            if (string.IsNullOrEmpty(defaultRdbms)) throw new System.Exception("Parameter 'defaultRdbms' could not be null or empty"); 
            DefaultRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(defaultRdbms); 
        }
        /// <summary>
        /// Sets Active RDBMS 
        /// </summary>
        public void SetActiveRdbms(string activeRdbms) 
        {
            if (string.IsNullOrEmpty(activeRdbms)) throw new System.Exception("Parameter 'activeRdbms' could not be null or empty"); 
            ActiveRdbms = RepoHelper.EnumEncoder.GetRdbmsEnum(activeRdbms); 
        }

        /// <summary>
        /// Sets parameter Host for database connection 
        /// </summary>
        public void SetDbHost(string dbHost) 
        {
            DbHost = dbHost; 
        }
        /// <summary>
        /// Sets parameter Port for database connection 
        /// </summary>
        public void SetDbName(string dbName) 
        {
            DbName = dbName; 
        }
        /// <summary>
        /// Sets parameter Name for database connection 
        /// </summary>
        public void SetDbPort(string dbPort) 
        {
            DbPort = dbPort; 
        }
        /// <summary>
        /// Sets parameter Schema for database connection 
        /// </summary>
        public void SetDbSchema(string dbSchema) 
        {
            DbSchema = dbSchema; 
        }
        /// <summary>
        /// Sets parameter Username for database connection 
        /// </summary>
        public void SetDbUsername(string dbUsername) 
        {
            DbUsername = dbUsername; 
        }
        /// <summary>
        /// Sets parameter Password for database connection 
        /// </summary>
        public void SetDbPassword(string dbPassword) 
        {
            DbPassword = dbPassword; 
        }
        #endregion  // Public methods 
    }
}
