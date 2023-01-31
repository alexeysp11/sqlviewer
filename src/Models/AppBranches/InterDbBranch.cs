using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// 
    /// </summary>
    public class InterDbBranch
    {
        /// <summary>
        /// 
        /// </summary>
        public DbInterconnection DbInterconnection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public InterDbBranch()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
