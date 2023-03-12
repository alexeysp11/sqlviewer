using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// Performs table related operations for PostgreSQL
    /// </summary>
    public class PgTablePreproc : IDbTablePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PgTablePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Gets SQL request for retrieving columns of a table 
        /// </summary>
        public string GetColumnsOfTableSql(string tableName)
        {
            try 
            {
                string[] tn = tableName.Split('.');
                return string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetColumns.sql"), tn[0], tn[1]); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets SQL request for retrieving foreign keys of a table 
        /// </summary>
        public string GetForeignKeysSql(string tableName)
        {
            try
            {
                return string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetForeignKeys.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets SQL request for retrieving triggers of a table 
        /// </summary>
        public string GetTriggersSql(string tableName)
        {
            try
            {
                return string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetTriggers.sql"), tableName); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets SQL request for retrieving definition of a table 
        /// </summary>
        public string GetTableDefinitionSql(string tableName)
        {
            try
            {
                string[] tn = tableName.Split('.');
                return string.Format(MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetSqlDefinition.sql"), tn[0], tn[1]);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
