using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using Microsoft.Win32;
using SqlViewer.Models.DbConnections; 
using SqlViewer.Models.DbPreproc; 
using SqlViewer.Models.DbTransfer; 
using SqlViewer.Helpers; 
using SqlViewer.Views; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.ViewModels
{
    public class DataVM
    {
        private MainVM MainVM { get; set; }

        public IDbPreproc AppRdbmsPreproc { get; private set; }
        public IDbPreproc UserRdbmsPreproc { get; private set; }
        
        public DbInterconnection DbInterconnection { get; private set; }

        public DataVM(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppRdbmsPreproc = new SqliteDbPreproc(this.MainVM); 
            this.DbInterconnection = new DbInterconnection(); 
        }

        #region Primary DB operations 
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
        /// Opens DB 
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
        #endregion  // Primary DB operations 

        #region Initialization 
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

                    case RdbmsEnum.MSSQL: 
                        (this.UserRdbmsPreproc = new MssqlDbPreproc(this.MainVM)).InitUserDbConnection(); 
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
        #endregion  // Initialization 

        #region // DB information 
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
        #endregion  // DB information 

        #region Low-level operations
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
        #endregion  // Low-level operations 
    }
}
