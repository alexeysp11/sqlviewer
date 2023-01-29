using System.Data; 
using SqlViewer.Helpers;
using Npgsql;

namespace SqlViewer.Models.DbConnections
{
    public class PgDbConnection : BaseDbConnection, ICommonDbConnection
    {
        private string DataSource { get; set; }

        public PgDbConnection() { }

        public PgDbConnection(string dataSource)
        {
            DataSource = dataSource; 
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 

            string connString = System.String.Format("Server={0};Username={1};Database={2};Port={3};Password={4}", 
                RepoHelper.AppSettingsRepo.DbHost,
                RepoHelper.AppSettingsRepo.DbUsername,
                RepoHelper.AppSettingsRepo.DbName,
                RepoHelper.AppSettingsRepo.DbPort,
                RepoHelper.AppSettingsRepo.DbPassword);

            using (var conn = new NpgsqlConnection(string.IsNullOrEmpty(DataSource) ? connString : DataSource))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sqlRequest, conn))
                {
                    var reader = command.ExecuteReader();
                    table = GetDataTable(reader); 
                    reader.Close();
                }
            }
            return table; 
        }

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return base.GetSqlFromDataTable(dt, tableName); 
        }

        private DataTable GetDataTable(NpgsqlDataReader reader)
        {
            DataTable table = new DataTable(); 
            if (reader.FieldCount == 0) return table; 
            for (int i = 0; i < reader.FieldCount; i++)
            {
                DataColumn column; 
                column = new DataColumn();
                column.ColumnName = reader.GetName(i);
                column.ReadOnly = true;
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
}
