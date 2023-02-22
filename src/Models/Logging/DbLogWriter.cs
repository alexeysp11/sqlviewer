using System.Collections.Generic; 
using SqlViewer.Helpers; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.Logging
{
    public class DbLogWriter : ILoggingWriter
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ICommonDbConnectionSV> DbConnections { get; protected set; }

        /// <summary>
        /// Constructor of DbLogWriter
        /// </summary>
        public DbLogWriter(List<ICommonDbConnectionSV> dbConnections) => DbConnections = dbConnections; 

        /// <summary>
        /// 
        /// </summary>
        public void WriteLog(string msg)
        {
            foreach (ICommonDbConnectionSV dbConnection in DbConnections)
            {
                string sqlRequest = System.IO.File.ReadAllText(System.IO.Path.Combine(SettingsHelper.GetRootFolder(), "extensions/SqlViewerDatabase/Queries/Sqlite/App/InsertIntoDbgLog.sql")); 
                sqlRequest = string.Format(sqlRequest, msg.Replace("'", "''")); 
                dbConnection.ExecuteSqlCommand(sqlRequest); 
            }
        }
    }
}
