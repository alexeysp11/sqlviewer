using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public class MysqlDbConnection : IDbConnection
    {
        private System.String ConnString { get; set; }

        public void SetConnString(System.String host, System.String username, System.String database)
        {
            this.ConnString = $"Host={host};Username={username};Database={database}"; 
        }
        
        public DataTable ExecuteSqlCommand(System.String sqlRequest)
        {
            DataTable table = new DataTable(); 
            
            return table; 
        }
    }
}