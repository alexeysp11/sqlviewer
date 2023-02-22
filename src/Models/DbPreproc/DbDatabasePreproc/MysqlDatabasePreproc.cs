using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class MysqlDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MysqlDatabasePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// 
        /// </summary>
        public void CreateDb()
        {
            System.Windows.MessageBox.Show("MySQL CreateDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("MySQL OpenDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\DisplayTablesInDb.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName); 
                return MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// 
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
