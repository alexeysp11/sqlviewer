using System.Data; 
using SqlViewer.Helpers;
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    public abstract class BaseDodbPreproc
    {
        public ICommonDbConnectionSV AppDbConnection { get; protected set; }
        public ICommonDbConnectionSV UserDbConnection { get; protected set; }
    }
}
