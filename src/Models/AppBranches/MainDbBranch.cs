using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using Microsoft.Win32;
using SqlViewer.Models.Creational.DbPreproc; 
using SqlViewer.Models.DbPreproc; 
using SqlViewer.Models.DbTransfer; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// Class for using database connections on SQL and tables pages 
    /// </summary>
    public class MainDbBranch
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public DbConnectionPreproc DbConnectionPreproc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DatabasePreproc DatabasePreproc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public TablePreproc TablePreproc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public RequestPreproc RequestPreproc { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CenterDbPreprocFactory CenterDbPreprocFactory { get; private set; } 
        #endregion  // Properties

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MainDbBranch(MainVM mainVM)
        {
            DbConnectionPreproc = new DbConnectionPreproc(mainVM); 
            DatabasePreproc = new DatabasePreproc(mainVM); 
            TablePreproc = new TablePreproc(mainVM); 
            RequestPreproc = new RequestPreproc(mainVM); 
            CenterDbPreprocFactory = new CenterDbPreprocFactory(mainVM); 
        }
        #endregion  // Constructor

        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV GetAppDbConnection()
        {
            return DbConnectionPreproc.AppDbConnection; 
        }
        /// <summary>
        /// 
        /// </summary>
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return DbConnectionPreproc.UserDbConnection; 
        }
    }
}
