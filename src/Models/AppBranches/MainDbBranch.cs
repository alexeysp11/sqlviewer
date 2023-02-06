using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using Microsoft.Win32;
using SqlViewerDatabase.DbConnections; 
using SqlViewer.Models.DbPreproc; 
using SqlViewer.Models.DbTransfer; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewer.Views; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// Class for using database connections on SQL and tables pages 
    /// </summary>
    public class MainDbBranch
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Instance of database connection for getting and/or setting app settings 
        /// </summary>
        public IDbPreproc AppRdbmsPreproc { get; private set; }
        /// <summary>
        /// Instance of database connection that performs queries written by a user 
        /// </summary>
        public IDbPreproc UserRdbmsPreproc { get; private set; }
        #endregion  // Properties

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MainDbBranch(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppRdbmsPreproc = new SqliteDbPreproc(this.MainVM); 
        }
        #endregion  // Constructor

        #region Public methods
        /// <summary>
        /// Creates database using UserRdbmsPreproc
        /// </summary>
        public void CreateDb()
        {
            try
            {
                UserRdbmsPreproc.CreateDb(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Opens database using UserRdbmsPreproc
        /// </summary>
        public void OpenDb()
        {
            try
            {
                InitUserDbConnection(); 
                UserRdbmsPreproc.OpenDb(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Gets SQL query from a local file 
        /// </summary>
        public string GetSqlRequest(string filename)
        {
            string sqlRequest = string.Empty; 
            try
            {
                sqlRequest = System.IO.File.ReadAllText($"{SettingsHelper.GetRootFolder()}\\src\\Queries\\{filename}"); 
            }
            catch (System.Exception ex) 
            {
                throw ex; 
            }
            return sqlRequest; 
        }

        /// <summary>
        /// initializes UserRdbmsPreproc
        /// </summary>
        public void InitUserDbConnection()
        {
            try
            {
                switch (RepoHelper.AppSettingsRepo.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        (this.UserRdbmsPreproc = new SqliteDbPreproc(this.MainVM)).InitUserDbConnection(); 
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        (this.UserRdbmsPreproc = new PgDbPreproc(this.MainVM)).InitUserDbConnection(); 
                        break;

                    case RdbmsEnum.MySQL: 
                        (this.UserRdbmsPreproc = new MysqlDbPreproc(this.MainVM)).InitUserDbConnection(); 
                        break;

                    case RdbmsEnum.Oracle: 
                        (this.UserRdbmsPreproc = new OracleDbPreproc(this.MainVM)).InitUserDbConnection(); 
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.ActiveRdbms}.");
                        break;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Displays all tables that a database contains 
        /// </summary>
        public void DisplayTablesInDb()
        {
            try
            {
                UserRdbmsPreproc.DisplayTablesInDb(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets all data from a table 
        /// </summary>
        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                UserRdbmsPreproc.GetAllDataFromTable(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the columns that a table contains 
        /// </summary>
        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                UserRdbmsPreproc.GetColumnsOfTable(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the foreign keys that a table contains 
        /// </summary>
        public void GetForeignKeys(string tableName)
        {
            try
            {
                UserRdbmsPreproc.GetForeignKeys(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the triggers that a table contains 
        /// </summary>
        public void GetTriggers(string tableName)
        {
            try
            {
                UserRdbmsPreproc.GetTriggers(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets SQL query for recreating a table  
        /// </summary>
        public void GetSqlDefinition(string tableName)
        {
            try
            {
                UserRdbmsPreproc.GetSqlDefinition(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        
        /// <summary>
        /// Sends SQL quty to database 
        /// </summary>
        public void SendSqlRequest()
        {
            try
            {
                UserRdbmsPreproc.SendSqlRequest(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Sends SQL quty to database and gets a result in a form of DataTable 
        /// </summary>
        public DataTable SendSqlRequest(string sql)
        {
            DataTable dt = new DataTable(); 
            try
            {
                dt = AppRdbmsPreproc.SendSqlRequest(sql); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            return dt; 
        }

        /// <summary>
        /// Clears temporary tables
        /// </summary>
        public void ClearTempTable(string tableName)
        {
            try
            {
                AppRdbmsPreproc.ClearTempTable(tableName); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // Public methods
    }
}
