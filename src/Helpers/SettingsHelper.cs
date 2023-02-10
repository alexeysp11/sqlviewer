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
        /// <summary>
        /// Gets filter for opening a local file  
        /// </summary>
        public static string GetFilterFileSystemString()
        {
            return "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3|Excel Workbook (2007 and later)|.xlsx|Excel Workbook (97-2003)|*.xls|Excel Template|.xltx|Excel 97-2003 template|*.xlt|Excel Macro-Enabled Workbook|.xlsm|ODS|*.ods|XML|*.xml|JSON|*.json|CSV|*.csv";
        }

        /// <summary>
        /// Gets a root folder of the project  
        /// </summary>
        public static string GetRootFolder()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\.."; 
        }

        /// <summary>
        /// Gets the folder where extensions are located 
        /// </summary>
        public static string GetExtensionsFolder()
        {
            return GetRootFolder() + "\\extension"; 
        }

        /// <summary>
        /// Gets string name of the active database management system 
        /// </summary>
        public static string GetActiveRdbmsString() 
        {
            return string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.ActiveRdbms.ToString()) ? RdbmsEnum.SQLite.ToString() : RepoHelper.AppSettingsRepo.ActiveRdbms.ToString(); 
        } 

        /// <summary>
        /// Gets a name of a temporary table in a form 'tmp202301302201'
        /// </summary>
        public static string GetTmpTableTransferString()
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

        /// <summary>
        /// Translates UI element 
        /// </summary>
        public static string TranslateUiElement(string eng, string translation)
        {
            return RepoHelper.AppSettingsRepo.Language == LanguageEnum.English || string.IsNullOrEmpty(translation) ? eng : translation;
        }

        #region Strings for helping users
        /// <summary>
        /// Gets information on how to form DataSource string to connect to a database
        /// </summary>
        public static string GetHelpDataSourceString()
        {
            return @"SQLite: specify the full path to the local database (for example, 'C:\projects\sqlviewer\data\app.db').

PostgreSQL: specify server, username, database, port and password (for example, 'Server=localhost;Username=username;Database=database;Port=800;Password=password').

MySQL: specify server, database, username, password (for example, Server=localhost; database=your_db; UID=username; password=password). 

Oracle: specify protocol, host, port, service name, user ID and password (for example, 'Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = service_name)
    )
  ); User ID=user_id;Password=password;')."; 
        }

        /// <summary>
        /// Gets explanation on how to interpret IP address of the computer 
        /// </summary>
        public static string GetHelpThisPcAddressString()
        {
            return "Network address of this PC contains IP and port (for example, '127.0.0.1:8000')"; 
        } 

        /// <summary>
        /// Gets info on why communication protocol is used in the application 
        /// </summary>
        public static string GetHelpCommunicationProtocolString()
        {
            return "Communication protocol allows you to communicate with another instance of this application installed on another PC, getting and receiving system messages."; 
        } 

        /// <summary>
        /// Gets info on why transfer protocol is used in the application 
        /// </summary>
        public static string GetHelpTransferProtocolString()
        {
            return "Transfer protocol allows you to communicate with another instance of this application installed on another PC, getting and receiving XML/JSON/CSV strings, spreadsheet (or local DB) files and database tables."; 
        } 
        #endregion  // Strings for helping users
    }
}
