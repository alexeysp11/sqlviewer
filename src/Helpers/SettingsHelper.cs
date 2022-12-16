using SqlViewer.Models.EnumOperations; 
using LanguageEnum = SqlViewer.Enums.Common.Language; 
using AutoSaveEnum = SqlViewer.Enums.Editor.AutoSave; 
using FontSizeEnum = SqlViewer.Enums.Editor.FontSize; 
using FontFamilyEnum = SqlViewer.Enums.Editor.FontFamily; 
using TabSizeEnum = SqlViewer.Enums.Editor.TabSize; 
using WordWrapEnum = SqlViewer.Enums.Editor.WordWrap; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Helpers
{
    /// <summary>
    /// Class for application settings 
    /// </summary>
    public static class SettingsHelper
    {
        public static string GetFilterFileSystemDb()
        {
            return "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";
        }

        public static string GetRootFolder()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\.."; 
        }

        public static void GetLanguage()
        {

        }

        public static void SetLanguage()
        {

        }
    }
}