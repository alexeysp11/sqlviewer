using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

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
