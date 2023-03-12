using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// Performs database connection related operations for Oracle
    /// </summary>
    public class OracleDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OracleDatabasePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Creates database 
        /// </summary>
        public void CreateDb()
        {
            //System.Windows.MessageBox.Show("Oracle CreateDb");
        }
        /// <summary>
        /// Opens database 
        /// </summary>
        public void OpenDb()
        {
            //System.Windows.MessageBox.Show("Oracle OpenDb");
        }
        /// <summary>
        /// Displays tables in the database 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\DisplayTablesInDb.sql"); 
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
            return $@"Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost})(PORT = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort}))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbName})
    )
  ); User ID={RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername};Password={RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword};"; 
        }
    }
}
