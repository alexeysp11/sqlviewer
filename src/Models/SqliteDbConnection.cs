using System.Data; 
using Microsoft.Data.Sqlite;

namespace SqlViewer.Models.Database
{
    public class SqliteDbConnection
    {
        public static SqliteDbConnection Instance { get; private set; } 

        private string absolutePathToDb;
        public string AbsolutePathToDb
        {
            get { return absolutePathToDb; }
            set { absolutePathToDb = value; }
        }
        
        static SqliteDbConnection()
        {
            Instance = new SqliteDbConnection(); 
        }

        private SqliteDbConnection() {}

        public void SetPathToDb(string path)
        {
            try
            {
                this.AbsolutePathToDb = path; 
            }
            catch (System.Exception e)
            {
                throw e;
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
                catch (System.Exception e)
                {
                    throw e; 
                }
            }
            return table; 
        }
    }
}