using System.Data; 
using Oracle.ManagedDataAccess.Client;

namespace SqlViewer.Models.DbConnections
{
    public class OracleDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection
    {
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            using (OracleConnection con = new OracleConnection(SqlViewer.Helpers.SettingsHelper.GetOracleConnectionString()))
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

        public string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return ""; 
        }
    }
}
