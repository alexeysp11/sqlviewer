using System.Data; 
using SqlViewer.Helpers; 
using SqlViewer.Models.AppBranches; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// Performs database related operations 
    /// </summary>
    public class DatabasePreproc
    {
        /// <summary>
        /// 
        /// </summary>
        public MainDbBranch MainDbBranch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DatabasePreproc(MainDbBranch mainDbBranch)
        {
            MainDbBranch = mainDbBranch; 
        }

        /// <summary>
        /// Creates database using UserRdbmsPreproc
        /// </summary>
        public void CreateDb()
        {
            try
            {
                MainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().CreateDb();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        /// <summary>
        /// Opens database using UserRdbmsPreproc
        /// </summary>
        public void OpenDb()
        {
            try
            {
                MainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().OpenDb();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Displays all tables that a database contains 
        /// </summary>
        public DataTable DisplayTablesInDb()
        {
            if (MainDbBranch.DbConnectionPreproc.UserDbConnection == null)
                return new DataTable(); 
            try
            {
                return MainDbBranch.CenterDbPreprocFactory.DatabasePreprocFactory.GetDbDatabasePreproc().DisplayTablesInDb();
                
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
    }
}
