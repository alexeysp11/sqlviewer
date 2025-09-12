using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// Performs table related operations for SQLite
    /// </summary>
    public class SqliteTablePreproc : IDbTablePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of SqliteTablePreproc
        /// </summary>
        public SqliteTablePreproc(MainVM mainVM) => MainVM = mainVM; 

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
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\GetSqlDefinition.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
