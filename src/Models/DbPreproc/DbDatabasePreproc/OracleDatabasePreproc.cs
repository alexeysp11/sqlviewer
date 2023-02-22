using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OracleDatabasePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// 
        /// </summary>
        public void CreateDb()
        {
            System.Windows.MessageBox.Show("Oracle CreateDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("Oracle OpenDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\DisplayTablesInDb.sql"); 
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
