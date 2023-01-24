using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppClients
{
    public class InterDbClient
    {
        public DbInterconnection DbInterconnection { get; private set; }

        public InterDbClient()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
