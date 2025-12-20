using System.Data;
using SqlViewer.Helpers;
using Npgsql;

namespace SqlViewer.Models.DbConnections;

public class PgDbConnection : ICommonDbConnection
{
    private string DataSource { get; set; }

    public PgDbConnection() { }

    public PgDbConnection(string dataSource)
    {
        DataSource = dataSource;
    }

    public DataTable ExecuteSqlCommand(string sqlRequest)
    {
        DataTable table = new();

        string connString = System.String.Format("Server={0};Username={1};Database={2};Port={3};Password={4}", 
            RepoHelper.AppSettingsRepo.DbHost,
            RepoHelper.AppSettingsRepo.DbUsername,
            RepoHelper.AppSettingsRepo.DbName,
            RepoHelper.AppSettingsRepo.DbPort,
            RepoHelper.AppSettingsRepo.DbPassword);

        using (NpgsqlConnection conn = new(string.IsNullOrEmpty(DataSource) ? connString : DataSource))
        {
            conn.Open();
            using (NpgsqlCommand command = new(sqlRequest, conn))
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                table = GetDataTable(reader);
                reader.Close();
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
        foreach(DataRow row in dt.Rows)
        {
            i = 0;
            sqlRequest += sqlInsert;
            foreach(DataColumn column in dt.Columns)
            {
                sqlRequest += "'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\n");
                i += 1;
            }
        }
        return sqlRequest;
    }

    private static DataTable GetDataTable(NpgsqlDataReader reader)
    {
        DataTable table = new();
        if (reader.FieldCount == 0) return table;
        for (int i = 0; i < reader.FieldCount; i++)
        {
            DataColumn column;
            column = new DataColumn
            {
                ColumnName = reader.GetName(i),
                ReadOnly = true
            };
            table.Columns.Add(column);
        }
        while (reader.Read())
        {
            DataRow row = table.NewRow();
            for (int i = 0; i < reader.FieldCount; i++)
                row[i] = reader.GetValue(i).ToString();
            table.Rows.Add(row);
        }
        return table;
    }
}
