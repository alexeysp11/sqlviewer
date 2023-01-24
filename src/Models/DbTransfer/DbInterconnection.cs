using SqlViewer.Models.DbConnections;
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbTransfer
{
    public class DbInterconnection
    {
        public ICommonDbConnectionSV InterDbConnection1 { get; private set; }
        public ICommonDbConnectionSV InterDbConnection2 { get; private set; }
        public ICommonDbConnectionSV NetworkDbConnection { get; private set; }

        public void SetInterDbConnection1(ICommonDbConnectionSV dbConnection) => InterDbConnection1 = dbConnection; 
        public void SetInterDbConnection2(ICommonDbConnectionSV dbConnection) => InterDbConnection2 = dbConnection; 
        public void SetNetworkDbConnection(ICommonDbConnectionSV dbConnection) => NetworkDbConnection = dbConnection; 

        public ICommonDbConnectionSV GetDbConnection(string activeRdbms, string dataSource)
        {
            if (string.IsNullOrEmpty(activeRdbms))
                throw new System.Exception("String 'ActiveRdbms' could not be null or empty"); 
            
            ICommonDbConnectionSV dbConnection; 
            switch (activeRdbms)
            {
                case "SQLite":
                    dbConnection = new SqliteDbConnection(dataSource); 
                    break;
            
                case "PostgreSQL":
                    dbConnection = new PgDbConnection(dataSource); 
                    break;
            
                case "MySQL":
                    dbConnection = new MysqlDbConnection(dataSource); 
                    break;
            
                case "Oracle":
                    dbConnection = new OracleDbConnection(dataSource); 
                    break;

                default: 
                    throw new System.Exception("Incorrect ActiveRdbms: '" + activeRdbms + "'"); 
                    break; 
            }
            return dbConnection; 
        }
    }
}
