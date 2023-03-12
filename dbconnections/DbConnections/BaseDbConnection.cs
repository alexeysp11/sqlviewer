using System.Data; 

namespace SqlViewerDatabase.DbConnections
{
    public class BaseDbConnection 
    {
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
