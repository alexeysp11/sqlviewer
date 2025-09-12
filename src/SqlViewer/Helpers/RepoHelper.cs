using SqlViewer.Models.DataStorage; 
using SqlViewer.Models.EnumOperations; 
using SqlViewer.Models.Logging; 

namespace SqlViewer.Helpers
{
    /// <summary>
    /// Helper for storing commonly used classes 
    /// </summary>
    public static class RepoHelper
    {
        /// <summary>
        /// App settings at the current moment 
        /// </summary>
        public static AppSettingsRepo AppSettingsRepo { get; private set; } = null; 

        /// <summary>
        /// Class for managing logging functionality 
        /// </summary>
        public static LoggingHub LoggingHub { get; private set; } = new LoggingHub(); 
        /// <summary>
        /// Class for getting enums from string 
        /// </summary>
        public static EnumEncoder EnumEncoder { get; private set; } = new EnumEncoder(); 
        /// <summary>
        /// Class for getting strings from enums
        /// </summary>
        public static EnumDecoder EnumDecoder { get; private set; } = new EnumDecoder(); 

        /// <summary>
        /// Sets an instance of a class for app settings at the current moment 
        /// </summary>
        public static void SetAppSettingsRepo(AppSettingsRepo appSettingsRepo) => AppSettingsRepo = appSettingsRepo; 
    }
}
