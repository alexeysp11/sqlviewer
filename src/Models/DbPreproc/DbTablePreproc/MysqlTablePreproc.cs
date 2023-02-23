using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// Performs table related operations for MySQL
    /// </summary>
    public class MysqlTablePreproc : IDbTablePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of MysqlTablePreproc
        /// </summary>
        public MysqlTablePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// Gets SQL request for retrieving columns of a table 
        /// </summary>
        public string GetColumnsOfTableSql(string tableName)
        {
            try
            {
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetColumns.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName, tableName); 
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
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetForeignKeys.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName, tableName);;
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
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetTriggers.sql"), tableName);
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
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetSqlDefinition.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
