using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// Performs database connection related operations for MySQL
    /// </summary>
    public class MysqlDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of MysqlDatabasePreproc
        /// </summary>
        public MysqlDatabasePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// Creates database 
        /// </summary>
        public void CreateDb()
        {
            System.Windows.MessageBox.Show("MySQL CreateDb");
        }
        /// <summary>
        /// Opens database 
        /// </summary>
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("MySQL OpenDb");
        }
        /// <summary>
        /// Displays tables in the database 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\DisplayTablesInDb.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName); 
                return MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest).DataTableResult;
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
