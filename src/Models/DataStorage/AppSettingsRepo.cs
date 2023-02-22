using SqlViewer.Helpers; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 

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
        /// 
        /// </summary>
        public DatabaseSettings DatabaseSettings { get; private set; } = new DatabaseSettings(); 
        /// <summary>
        /// 
        /// </summary>
        public EditorSettings EditorSettings { get; private set; } = new EditorSettings(); 

        /// <summary>
        /// Language of the application 
        /// </summary>
        public LanguageEnum Language { get; private set; }
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
        #endregion  // Public methods 
    }
}
