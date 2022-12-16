using System.Data; 

namespace SqlViewer.Models.DbConnections
{
    public interface ICommonDbConnection
    {
        DataTable ExecuteSqlCommand(string sqlRequest); 
    }
}