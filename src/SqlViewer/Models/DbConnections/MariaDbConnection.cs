using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public class MariadbDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection
    {
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }

        public string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return ""; 
        }
    }
}
