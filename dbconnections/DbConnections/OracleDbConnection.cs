using System.Data; 
using Oracle.ManagedDataAccess.Client;

namespace SqlViewerDatabase.DbConnections
{
    public class OracleDbConnection : BaseDbConnection, ICommonDbConnection
    {
        private string DataSource { get; set; }
        private string ConnString { get; set; }
        
        public OracleDbConnection() { }

        public OracleDbConnection(string dataSource)
        {
            DataSource = dataSource; 
        }

        public ICommonDbConnection SetConnString(string connString)
        {
            ConnString = connString; 
            return this; 
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            using (OracleConnection con = new OracleConnection(string.IsNullOrEmpty(DataSource) ? ConnString : DataSource))
            {
                using (OracleCommand cmd = new OracleCommand(sqlRequest, con))
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                }
            }
            return table;
        }

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return base.GetSqlFromDataTable(dt, tableName); 
        }
    }
}
