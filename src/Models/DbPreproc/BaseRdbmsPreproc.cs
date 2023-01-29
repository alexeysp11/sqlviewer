using System.Data; 
using SqlViewer.Helpers;
using SqlViewer.Models.DbConnections; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbPreproc
{
    public abstract class BaseRdbmsPreproc
    {
        public ICommonDbConnectionSV AppDbConnection { get; protected set; }
        public ICommonDbConnectionSV UserDbConnection { get; protected set; }
    }
}
