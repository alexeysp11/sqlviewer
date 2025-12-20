using System.Data;

namespace SqlViewer.Models.DbConnections;

public class MariadbDbConnection : ICommonDbConnection
{
    public DataTable ExecuteSqlCommand(string sqlRequest)
    {
        DataTable table = new();
        
        return table;
    }

    public string GetSqlFromDataTable(DataTable dt, string tableName)
    {
        return "";
    }
}
