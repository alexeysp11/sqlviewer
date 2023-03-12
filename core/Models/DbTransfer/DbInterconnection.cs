using SqlViewerDatabase.DbConnections;
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbTransfer
{
    /// <summary>
    /// Intermediatory layer between InterDbBranch and databases 
    /// </summary>
    public class DbInterconnection
    {
        /// <summary>
        /// Implementation of ICommonDbConnection interface for the first one connection 
        /// </summary>
        public ICommonDbConnectionSV InterDbConnection1 { get; private set; }
        /// <summary>
        /// Implementation of ICommonDbConnection interface for the second one connection 
        /// </summary>
        public ICommonDbConnectionSV InterDbConnection2 { get; private set; }

        /// <summary>
        /// Sets InterDbConnection1
        /// </summary>
        public void SetInterDbConnection1(ICommonDbConnectionSV dbConnection) => InterDbConnection1 = dbConnection; 
        /// <summary>
        /// Sets InterDbConnection2
        /// </summary>
        public void SetInterDbConnection2(ICommonDbConnectionSV dbConnection) => InterDbConnection2 = dbConnection; 

        /// <summary>
        /// Gets an instance that implements ICommonDbConnection, which a user is supposed to connect to 
        /// </summary>
        public ICommonDbConnectionSV GetDbConnection(string activeRdbms, string dataSource)
        {
            if (string.IsNullOrEmpty(activeRdbms))
                throw new System.Exception("String 'ActiveRdbms' could not be null or empty"); 
            
            ICommonDbConnectionSV dbConnection; 
            switch (activeRdbms)
            {
                case nameof(RdbmsEnum.SQLite):
                    dbConnection = new SqliteDbConnection(dataSource); 
                    break;
            
                case nameof(RdbmsEnum.PostgreSQL):
                    dbConnection = new PgDbConnection(dataSource); 
                    break;
            
                case nameof(RdbmsEnum.MySQL):
                    dbConnection = new MysqlDbConnection(dataSource); 
                    break;
            
                case nameof(RdbmsEnum.Oracle):
                    dbConnection = new OracleDbConnection(dataSource); 
                    break;

                default: 
                    throw new System.Exception("Incorrect ActiveRdbms: '" + activeRdbms + "'"); 
            }
            return dbConnection; 
        }
    }
}
