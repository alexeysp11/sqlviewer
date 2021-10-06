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

        private int[] MaxSizeOfColumns; 

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

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable(); 
            try
            {
                table = GetDataTable(sqlRequest);
            }
            catch (System.Exception e)
            {
                throw e; 
            }
            return table; 
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

        private void GetMaxSizeOfColumns(DataTable table)
        {
            MaxSizeOfColumns = new int[table.Columns.Count]; 

            int i = 0; 
            foreach (DataColumn column in table.Columns)
            {
                string columnName = column.ColumnName.ToString(); 
                MaxSizeOfColumns[i] = columnName.Length; 
                i++; 
            }

            foreach (DataRow row in table.Rows)
            {
                i = 0; 
                foreach (DataColumn column in table.Columns)
                {
                    string property = row[column].ToString(); 
                    MaxSizeOfColumns[i] = (MaxSizeOfColumns[i] < property.Length) ? property.Length : MaxSizeOfColumns[i]; 
                    i++; 
                }
            }
        }

        private void SetColumnNames(ref DataTable table, ref string result)
        {
            try
            {
                int index = 0; 
                foreach (DataColumn column in table.Columns)
                {
                    string columnName = column.ColumnName.ToString(); 
                    result += $"| {columnName} ";
                    AddExtraSpaces(ref result, columnName.Length, index);
                    index++; 
                }
                result += "|\n"; 
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        private void SetSeparator(DataTable table, ref string result)
        {
            try
            {
                int index = 0; 
                foreach (DataColumn column in table.Columns)
                {
                    result += $"|-";
                    for (int i = 0; i < MaxSizeOfColumns[index]; i++)
                    {
                        result += "-"; 
                    }
                    result += "-";
                    index++; 
                }
                result += "|\n"; 
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        private void SetTableContent(ref DataTable table, ref string result)
        {
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    int index = 0; 
                    foreach (DataColumn column in table.Columns)
                    {
                        string property = row[column].ToString(); 
                        result += $"| {property} "; 
                        AddExtraSpaces(ref result, property.Length, index);
                        index++; 
                    }
                    result += "|\n"; 
                }
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        private void AddExtraSpaces(ref string result, int stringLength, int index)
        {
            int length = MaxSizeOfColumns[index] - stringLength; 

            if (length < 0)
            {
                throw new System.Exception("Length of a string is bigger than maximum length in its column"); 
            }

            for (int i = 0; i < length; i++)
            {
                result += " "; 
            }
        }
    }
}