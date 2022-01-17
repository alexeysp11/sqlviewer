using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public class PgDbConnection : IDbConnection
    {
        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }
    }
}