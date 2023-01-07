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

        public static string GetExtensionsFolder()
        {
            return GetRootFolder() + "\\extension"; 
        }

        public static void GetLanguage()
        {

        }

        public static void SetLanguage()
        {

        }

        public static string GetActiveRdbmsString() 
        {
            return string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
        } 

        public static string GetTmpTableTransfer()
        {
            System.DateTime now = System.DateTime.UtcNow; 

            string year = now.Year.ToString(); 
            string month = (now.Month < 10 ? "0" : "")  + now.Month.ToString(); 
            string day = (now.Day < 10 ? "0" : "") + now.Day.ToString(); 
            string hour = (now.Hour < 10 ? "0" : "") + now.Hour.ToString(); 
            string minute = (now.Minute < 10 ? "0" : "") + now.Minute.ToString(); 
            string second = (now.Second < 10 ? "0" : "") + now.Second.ToString(); 

            return "tmp" + year + month + day + hour + minute + second; 
        }

        #region DB connection strings
        public static string GetPgDbConnectionString()
        {
            return ""; 
        }

        public static string GetOracleConnectionString()
        {
            return @"Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = service_name)
    )
  ); User ID=user_id;Password=password;"; 
        }
        #endregion  // DB connection strings
    }
}
