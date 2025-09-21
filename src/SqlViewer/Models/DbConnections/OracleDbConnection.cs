using System.Data; 
using SqlViewer.Helpers;
using Oracle.ManagedDataAccess.Client;

namespace SqlViewer.Models.DbConnections
{
    public class OracleDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection
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

        public string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            int i = 0; 
            string sqlRequest = "CREATE TABLE " + tableName + " ("; 
            string sqlInsert = "INSERT INTO " + tableName + " ("; 
            foreach (DataColumn column in dt.Columns)
            {
                sqlRequest += "\n" + column.ColumnName + " CLOB" + (i != dt.Columns.Count - 1 ? "," : ");\n"); 
                sqlInsert += column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ")\nVALUES ("); 
                i += 1; 
            }
            foreach(DataRow row in dt.Rows)
            {
                i = 0; 
                sqlRequest += sqlInsert; 
                foreach(DataColumn column in dt.Columns)
                {
                    sqlRequest += "'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\n"); 
                    i += 1; 
                }
            }
            return sqlRequest; 
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
