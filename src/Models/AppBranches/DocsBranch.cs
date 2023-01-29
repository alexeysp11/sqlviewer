using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    public class DocsBranch
    {
        public DbInterconnection DbInterconnection { get; private set; }

        public DocsBranch()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
