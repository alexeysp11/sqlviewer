using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class TablePreproc 
    {
        /// <summary>
        /// 
        /// </summary>
        private MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TablePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Gets all data from a table 
        /// </summary>
        public DataTable GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets info about all the columns that a table contains 
        /// </summary>
        public DataTable GetColumnsOfTable(string tableName)
        {
            try
            {
                string sqlRequest = MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetColumnsOfTableSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets info about all the foreign keys that a table contains 
        /// </summary>
        public DataTable GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetForeignKeysSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets info about all the triggers that a table contains 
        /// </summary>
        public DataTable GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetTriggersSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                return MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Gets SQL query for recreating a table  
        /// </summary>
        public string GetTableDefinition(string tableName)
        {
            try
            {
                string sqlRequest = MainDbBranch.CenterDbPreprocFactory.TablePreprocFactory.GetDbTablePreproc().GetTableDefinitionSql(tableName); 
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                DataTable dt = MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                return dt.Rows.Count > 0 ? dt.Rows[0]["sql"].ToString() : string.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        
    }
}
