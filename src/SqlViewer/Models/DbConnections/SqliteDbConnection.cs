using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SqlViewer.Models.DbConnections;

public class SqliteDbConnection : ICommonDbConnection
{
    private string AbsolutePathToDb;

    public SqliteDbConnection(string path)
    {
        SetPathToDb(path);
    }

    public void SetPathToDb(string path)
    {
        if (File.Exists(path))
        {
            AbsolutePathToDb = path;
        }
        else
        {
            throw new Exception($"Database file '{path}' does not exists");
        }
    }

    public DataTable ExecuteSqlCommand(string sqlRequest)
    {
        DataTable table = new();
        SqliteConnectionStringBuilder connectionStringBuilder = new()
        {
            DataSource = AbsolutePathToDb
        };
        using (SqliteConnection connection = new(connectionStringBuilder.ConnectionString))
        {
            connection.Open();
            SqliteCommand selectCmd = connection.CreateCommand();
            selectCmd.CommandText = sqlRequest;
            using (SqliteDataReader reader = selectCmd.ExecuteReader())
            {
                table.Load(reader);
            }
        }
        return table;
    }

    public string GetSqlFromDataTable(DataTable dt, string tableName)
    {
        int i = 0;
        string sqlRequest = "CREATE TABLE " + tableName + " (";
        string sqlInsert = "INSERT INTO " + tableName + " (";
        foreach (DataColumn column in dt.Columns)
        {
            sqlRequest += "\n" + column.ColumnName + " TEXT" + (i != dt.Columns.Count - 1 ? "," : ");\n");
            sqlInsert += column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ")\nVALUES (");
            i += 1;
        }
        foreach (DataRow row in dt.Rows)
        {
            i = 0;
            sqlRequest += sqlInsert;
            foreach (DataColumn column in dt.Columns)
            {
                sqlRequest += "'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\n");
                i += 1;
            }
        }
        return sqlRequest;
    }
}
