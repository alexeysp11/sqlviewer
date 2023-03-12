using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// Performs request related operations 
    /// </summary>
    public class RequestPreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RequestPreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Sends SQL quty to database 
        /// </summary>
        public DataTable SendSqlRequestU(string sql)
        {
            try
            {
                if (MainDbBranch.DbConnectionPreproc.UserDbConnection == null)
                    throw new System.Exception("Database is not opened."); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Sends SQL quty to database and gets a result in a form of DataTable 
        /// </summary>
        public DataTable SendSqlRequestA(string sql)
        {
            try
            {
                if (MainDbBranch.DbConnectionPreproc.AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                return MainDbBranch.DbConnectionPreproc.AppDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Gets SQL query from a local file 
        /// </summary>
        public string GetSqlRequestFromFile(string filename)
        {
            string sqlRequest = string.Empty; 
            try
            {
                sqlRequest = System.IO.File.ReadAllText($"{SettingsHelper.GetRootFolder()}/dbconnections/Queries/{filename}"); 
            }
            catch (System.Exception ex) 
            {
                throw ex; 
            }
            return sqlRequest; 
        }
    }
}
