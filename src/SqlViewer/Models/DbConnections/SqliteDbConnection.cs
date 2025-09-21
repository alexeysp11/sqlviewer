using System.Data; 
using Microsoft.Data.Sqlite;

namespace SqlViewer.Models.DbConnections
{
    public class SqliteDbConnection : SqlViewer.Models.DbConnections.ICommonDbConnection 
    {
        //public string ProviderName { get; private set; } // For debugging purposes

        private string AbsolutePathToDb; 

        public SqliteDbConnection(string path)
        {
            try
            {
                SetPathToDb(path); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public void SetPathToDb(string path)
        {
            if (System.IO.File.Exists(path))
            {
                this.AbsolutePathToDb = path; 
            }
            else
            {
                throw new System.Exception($"Database file '{path}' does not exists");
            }
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = AbsolutePathToDb;
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = sqlRequest; 
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex; 
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
    }
}
