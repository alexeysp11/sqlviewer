using System.Data; 
using Npgsql;

namespace SqlViewer.Models.DbConnections
{
    public class PgDbConnection : IDbConnection
    {
        private System.String ConnString { get; set; }

        public void SetConnString(System.String host, System.String username, System.String database)
        {
            this.ConnString = $"Host={host};Username={username};Database={database}"; 
        }

        public DataTable ExecuteSqlCommand(System.String sqlRequest)
        {
            DataTable table = new DataTable(); 

            NpgsqlConnection connection = new NpgsqlConnection(ConnString);
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sqlRequest;
                cmd.CommandType = System.Data.CommandType.Text;
                using (var dataReader = cmd.ExecuteReader())
                {
                    // Add new column into DataTable 
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        var column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = dataReader.GetName(i);
                        column.ReadOnly = true;
                        table.Columns.Add(column);
                    }

                    // Add a new row 
                    while (dataReader.Read())
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            row[i] = dataReader.GetString(i);
                        }
                        table.Rows.Add(row);
                    }
                    dataReader.Dispose();
                }
            }
            catch (System.Exception e)
            {
                throw e; 
            }
            finally
            {
                cmd.Dispose();
                connection.Dispose();
            }
            return table; 
        }
    }
}