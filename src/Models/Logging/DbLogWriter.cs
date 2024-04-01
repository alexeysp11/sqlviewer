using System.Collections.Generic; 
using SqlViewer.Helpers; 
using WorkflowLib.DbConnections; 
using ICommonDbConnectionSV = WorkflowLib.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.Logging
{
    /// <summary>
    /// Class for writing logs into database 
    /// </summary>
    public class DbLogWriter : ILoggingWriter
    {
        /// <summary>
        /// List of database connections 
        /// </summary>
        public List<ICommonDbConnectionSV> DbConnections { get; protected set; }

        /// <summary>
        /// Constructor of DbLogWriter
        /// </summary>
        public DbLogWriter(List<ICommonDbConnectionSV> dbConnections) => DbConnections = dbConnections; 

        /// <summary>
        /// Writing logs using the list of database connections 
        /// </summary>
        public void WriteLog(string msg)
        {
            foreach (ICommonDbConnectionSV dbConnection in DbConnections)
            {
                string sqlRequest = System.IO.File.ReadAllText(System.IO.Path.Combine(SettingsHelper.GetRootFolder(), "queries/Sqlite/App/InsertIntoDbgLog.sql")); 
                sqlRequest = string.Format(sqlRequest, msg.Replace("'", "''")); 
                dbConnection.ExecuteSqlCommand(sqlRequest); 
            }
        }
    }
}
