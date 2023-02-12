using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.Logging
{
    public class DbLogWriter : ILoggingWriter
    {
        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV DbConnection { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public void WriteLog(string msg)
        {

        }
    }
}
