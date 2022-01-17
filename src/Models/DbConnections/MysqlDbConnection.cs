using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public class MysqlDbConnection : IDbConnection
    {
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }
    }
}