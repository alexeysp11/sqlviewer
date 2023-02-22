using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class PgDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PgDatabasePreproc(MainVM mainVM) => MainVM = mainVM; 
        
        /// <summary>
        /// 
        /// </summary>
        public void CreateDb()
        {
            System.Windows.MessageBox.Show("PostgreSQL CreateDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("PostgreSQL OpenDb");
        }
        /// <summary>
        /// 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\DisplayTablesInDb.sql"); 
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
            return System.String.Format("Server={0};Username={1};Database={2};Port={3};Password={4}", 
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbName,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort,
                RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword); 
        }
    }
}
