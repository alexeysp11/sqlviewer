using System.Collections.Generic; 
using System.Data; 
using Microsoft.Data.Sqlite;

namespace SqlViewer.Models.Database
{
    public class SqliteDbHelper
    {
        public static SqliteDbHelper Instance { get; private set; } 

        private string absolutePathToDb;
        public string AbsolutePathToDb
        {
            get { return absolutePathToDb; }
            set { absolutePathToDb = value; }
        }

        static SqliteDbHelper()
        {
            Instance = new SqliteDbHelper(); 
        }

        private SqliteDbHelper() 
        { 

        }

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

        public string ExecuteSqlCommand(string sqlRequest)
        {
            string result = string.Empty; 
            try
            {
                DataTable table = GetDataTable(sqlRequest);
                foreach (DataColumn column in table.Columns)
                {
                    result += $"{column.ColumnName.ToString()}\t";
                }
                result += "\n"; 

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        result += $"{row[column].ToString()}\t"; 
                    }
                    result += "\n"; 
                }
            }
            catch (System.Exception e)
            {
                throw e; 
            }
            return result; 
        }

        private DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = AbsolutePathToDb;
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = sql; 
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        return dt;
                    }
                }
                catch (System.Exception e)
                {
                    throw e; 
                }
            }
        }
    }
}