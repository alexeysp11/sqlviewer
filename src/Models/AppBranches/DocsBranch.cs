using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// 
    /// </summary>
    public class DocsBranch
    {
        /// <summary>
        /// 
        /// </summary>
        public DbInterconnection DbInterconnection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DocsBranch()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
