using SqlViewerDatabase.DbConnections;
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.DbTransfer
{
    /// <summary>
    /// 
    /// </summary>
    public class DbInterconnection
    {
        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV InterDbConnection1 { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV InterDbConnection2 { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV NetworkDbConnection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void SetInterDbConnection1(ICommonDbConnectionSV dbConnection) => InterDbConnection1 = dbConnection; 
        /// <summary>
        /// 
        /// </summary>
        public void SetInterDbConnection2(ICommonDbConnectionSV dbConnection) => InterDbConnection2 = dbConnection; 
        /// <summary>
        /// 
        /// </summary>
        public void SetNetworkDbConnection(ICommonDbConnectionSV dbConnection) => NetworkDbConnection = dbConnection; 

        /// <summary>
        /// 
        /// </summary>
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
