using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class SqliteDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SqliteDatabasePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// 
        /// </summary>
        public void CreateDb()
        {
            SqlViewer.Helpers.FileSysHelper.SaveLocalFile(); 
        }
        /// <summary>
        /// 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                string path = SqlViewer.Helpers.FileSysHelper.OpenLocalFile(); 
                if (path == string.Empty) return; 
                MainVM.DataVM.MainDbBranch.DbConnectionPreproc.InitLocalDbConnection(path); 
                MainVM.DataVM.MainDbBranch.DatabasePreproc.DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\DisplayTablesInDb.sql"); 
                return MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
