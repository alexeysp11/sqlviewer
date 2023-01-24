using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppClients
{
    public class DocsClient
    {
        public DbInterconnection DbInterconnection { get; private set; }

        public DocsClient()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }
    }
}
