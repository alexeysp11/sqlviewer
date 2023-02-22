using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 

namespace SqlViewer.Models.DbPreproc.DbTablePreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleTablePreproc : IDbTablePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        /// <summary>
        /// Constructor of OracleTablePreproc
        /// </summary>
        public OracleTablePreproc(MainVM mainVM) => MainVM = mainVM; 

        /// <summary>
        /// 
        /// </summary>
        public string GetColumnsOfTableSql(string tableName)
        {
            try
            {
                string[] tn = tableName.Split('.');
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetColumns.sql"), tn[0], tn[1]); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetForeignKeysSql(string tableName)
        {
            try
            {
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetForeignKeys.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetTriggersSql(string tableName)
        {
            try
            {
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetTriggers.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetSqlDefinitionSql(string tableName)
        {
            try
            {
                return string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetSqlDefinition.sql"), tableName);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
