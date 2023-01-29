using System.Data; 

namespace SqlViewerDatabase.DbConnections
{
    public interface ICommonDbConnection
    {
        DataTable ExecuteSqlCommand(string sqlRequest); 
        
        string GetSqlFromDataTable(DataTable dt, string tableName); 

        ICommonDbConnection SetConnString(string connString);
    }
}
