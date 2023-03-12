using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// Performs table related operations for SQLite
    /// </summary>
    public class SqliteTablePreproc : IDbTablePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SqliteTablePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Gets SQL request for retrieving columns of a table 
        /// </summary>
        public string GetColumnsOfTableSql(string tableName)
        {
            return $"PRAGMA table_info({tableName});"; 
        }
        /// <summary>
        /// Gets SQL request for retrieving foreign keys of a table 
        /// </summary>
        public string GetForeignKeysSql(string tableName)
        {
            return $"PRAGMA foreign_key_list('{tableName}');"; 
        }
        /// <summary>
        /// Gets SQL request for retrieving triggers of a table 
        /// </summary>
        public string GetTriggersSql(string tableName)
        {
            return $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
        }
        /// <summary>
        /// Gets SQL request for retrieving definition of a table 
        /// </summary>
        public string GetTableDefinitionSql(string tableName)
        {
            try
            {
                return string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\GetSqlDefinition.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
