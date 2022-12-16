using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public class MysqlDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection
    {
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }
    }
}