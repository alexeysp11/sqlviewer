using System.Data; 
using SqlViewer.Helpers;
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// Abstract base class for preprocessing RDBMS connections 
    /// </summary>
    public abstract class BaseRdbmsPreproc
    {
        /// <summary>
        /// Database connection instance on the App layer  
        /// </summary>
        public ICommonDbConnectionSV AppDbConnection { get; protected set; }
        /// <summary>
        /// Database connection instance on the User layer  
        /// </summary>
        public ICommonDbConnectionSV UserDbConnection { get; protected set; }
    }
}
