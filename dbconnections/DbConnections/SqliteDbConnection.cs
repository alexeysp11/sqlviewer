using System.Data; 
using Microsoft.Data.Sqlite;

namespace SqlViewerDatabase.DbConnections
{
    public class SqliteDbConnection : BaseDbConnection, ICommonDbConnection 
    {
        private string ConnString { get; set; }
        private string AbsolutePathToDb { get; set; }

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

        public ICommonDbConnection SetConnString(string connString)
        {
            ConnString = connString; 
            return this; 
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

        public new string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return base.GetSqlFromDataTable(dt, tableName); 
        }
    }
}
