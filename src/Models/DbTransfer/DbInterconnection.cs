using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbTransfer
{
    public class DbInterconnection
    {
        public ICommonDbConnectionSV DbConnection1 { get; private set; }
        public ICommonDbConnectionSV DbConnection2 { get; private set; }

        public void SetDbConnection1(ICommonDbConnectionSV dbConnection) => DbConnection1 = dbConnection; 
        public void SetDbConnection2(ICommonDbConnectionSV dbConnection) => DbConnection2 = dbConnection; 
    }
}
