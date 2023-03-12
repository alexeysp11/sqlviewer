using SqlViewer.Models.DbTransfer; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// Branch of the application that allows to deal with two database connections
    /// </summary>
    public class InterDbBranch
    {
        /// <summary>
        /// Intermediatory layer between InterDbBranch and databases 
        /// </summary>
        public DbInterconnection DbInterconnection { get; private set; }

        /// <summary>
        /// Constructor of InterDbBranch, that initializes DbInterconnection
        /// </summary>
        public InterDbBranch() => this.DbInterconnection = new DbInterconnection(); 
    }
}
