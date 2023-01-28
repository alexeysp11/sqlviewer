using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

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
