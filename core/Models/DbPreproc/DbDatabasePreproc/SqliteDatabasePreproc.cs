using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.DbPreproc.DbDatabasePreproc
{
    /// <summary>
    /// Performs database connection related operations for SQLite
    /// </summary>
    public class SqliteDatabasePreproc : IDbDatabasePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SqliteDatabasePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Creates database 
        /// </summary>
        public void CreateDb()
        {
            //SqlViewer.Helpers.FileSysHelper.SaveLocalFile(); 
        }
        /// <summary>
        /// Opens database 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                //string path = SqlViewer.Helpers.FileSysHelper.OpenLocalFile(); 
                string path = ""; 
                if (path == string.Empty) return; 
                MainDbBranch.DbConnectionPreproc.InitLocalDbConnection(path); 
                MainDbBranch.DatabasePreproc.DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Displays tables in the database 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\DisplayTablesInDb.sql"); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
