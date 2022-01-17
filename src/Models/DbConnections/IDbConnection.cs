using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public interface IDbConnection
    {
        DataTable ExecuteSqlCommand(string sqlRequest); 
    }
}