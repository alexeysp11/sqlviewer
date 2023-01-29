using System.Data; 
using SqlViewer.Helpers;
using Oracle.ManagedDataAccess.Client;

namespace SqlViewer.Models.DbConnections
{
    public class OracleDbConnection : BaseDbConnection, ICommonDbConnection
    {
        private string DataSource { get; set; }
        
        public OracleDbConnection() { }

        public OracleDbConnection(string dataSource)
        {
            DataSource = dataSource; 
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            using (OracleConnection con = new OracleConnection(string.IsNullOrEmpty(DataSource) ? GetOracleConnectionString() : DataSource))
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
        
        private string GetOracleConnectionString()
        {
            return $@"Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = {RepoHelper.AppSettingsRepo.DbHost})(PORT = {RepoHelper.AppSettingsRepo.DbPort}))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = {RepoHelper.AppSettingsRepo.DbName})
    )
  ); User ID={RepoHelper.AppSettingsRepo.DbUsername};Password={RepoHelper.AppSettingsRepo.DbPassword};"; 
        }
    }
}
