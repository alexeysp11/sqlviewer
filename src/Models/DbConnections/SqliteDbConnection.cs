using System.Data; 
using Microsoft.Data.Sqlite;

namespace SqlViewer.Models.DbConnections
{
    public class SqliteDbConnection : SqlViewer.Models.DbConnections.IDbConnection 
    {
        //public System.String ProviderName { get; private set; } // For debugging purposes

        private System.String AbsolutePathToDb; 

        public SqliteDbConnection(System.String path)
        {
            try
            {
                SetPathToDb(path); 
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void SetConnString(System.String host, System.String username, System.String database)
        {
            throw new System.NotImplementedException("There's no connection System.String in SqliteDbConnection."); 
        }

        public void SetPathToDb(System.String path)
        {
            if ( System.IO.File.Exists(path) )
            {
                this.AbsolutePathToDb = path; 
            }
            else
            {
                throw new System.Exception($"Database file '{path}' does not exists");
            }
        }

        public DataTable ExecuteSqlCommand(System.String sqlRequest)
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
                catch (System.Exception e)
                {
                    throw e; 
                }
            }
            return table; 
        }
    }
}