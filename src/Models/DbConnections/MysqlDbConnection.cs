using System.Data; 
using SqlViewer.Helpers;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SqlViewer.Models.DbConnections
{
    public class MysqlDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection
    {
        private string DataSource { get; set; }

        public MysqlDbConnection() { }

        public MysqlDbConnection(string dataSource)
        {
            DataSource = dataSource; 
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            MySqlConnection connection = null; 
            try
            {
                string connString = string.Format("Server={0}; database={1}; UID={2}; password={3}",  
                    RepoHelper.AppSettingsRepo.DbHost,
                    RepoHelper.AppSettingsRepo.DbName,
                    RepoHelper.AppSettingsRepo.DbUsername,
                    //RepoHelper.AppSettingsRepo.DbPort,
                    RepoHelper.AppSettingsRepo.DbPassword);
                connection = new MySqlConnection(string.IsNullOrEmpty(DataSource) ? connString : DataSource);
                connection.Open();
                var reader = (new MySqlCommand(sqlRequest, connection)).ExecuteReader();
                table = GetDataTable(reader); 
            }
            catch (System.Exception e)
            {
                throw e; 
            }
            finally
            {
                if (connection != null)
                    connection.Close();
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

        private DataTable GetDataTable(MySqlDataReader reader)
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
