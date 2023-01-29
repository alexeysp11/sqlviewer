using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    public class InterDbBranch
    {
        public DbInterconnection DbInterconnection { get; private set; }

        public InterDbBranch()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
