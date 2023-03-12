using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// Performs database connection related operations for MySQL
    /// </summary>
    public class MysqlDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MysqlDatabasePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Creates database 
        /// </summary>
        public void CreateDb()
        {
            //System.Windows.MessageBox.Show("MySQL CreateDb");
        }
        /// <summary>
        /// Opens database 
        /// </summary>
        public void OpenDb()
        {
            //System.Windows.MessageBox.Show("MySQL OpenDb");
        }
        /// <summary>
        /// Displays tables in the database 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\DisplayTablesInDb.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets connection string 
        /// </summary>
        private string GetConnString()
        {
            return string.Format("Server={0}; database={1}; UID={2}; password={3}",  
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbName,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername,
                //RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword);
        }
    }
}
