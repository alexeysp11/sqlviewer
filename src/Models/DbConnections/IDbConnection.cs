using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public interface IDbConnection
    {
        void SetConnString(System.String host, System.String username, System.String database);
        DataTable ExecuteSqlCommand(System.String sqlRequest); 
    }
}