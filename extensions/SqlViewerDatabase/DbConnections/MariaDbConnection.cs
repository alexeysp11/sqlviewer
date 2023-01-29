using System.Data; 

namespace SqlViewerDatabase.DbConnections
{
    public class MariadbDbConnection : BaseDbConnection, ICommonDbConnection
    {
        private string ConnString { get; set; }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }

        public ICommonDbConnection SetConnString(string connString)
        {
            ConnString = connString; 
            return this; 
        }

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return base.GetSqlFromDataTable(dt, tableName); 
        }
    }
}
