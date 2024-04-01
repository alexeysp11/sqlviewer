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
using WorkflowLib.DbConnections; 
using ICommonDbConnectionSV = WorkflowLib.DbConnections.ICommonDbConnection; 

namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// Class for using database connections on SQL and tables pages 
    /// </summary>
    public class MainDbBranch
    {
        #region Properties
        /// <summary>
        /// Instance of preproc class that performs database related operations 
        /// </summary>
        public DbConnectionPreproc DbConnectionPreproc { get; private set; }
        /// <summary>
        /// Instance of preproc class that performs database connection related operations 
        /// </summary>
        public DatabasePreproc DatabasePreproc { get; private set; }
        /// <summary>
        /// Instance of preproc class that performs table related operations 
        /// </summary>
        public TablePreproc TablePreproc { get; private set; }
        /// <summary>
        /// Instance of preproc class that performs request related operations 
        /// </summary>
        public RequestPreproc RequestPreproc { get; private set; }
        /// <summary>
        /// Instance of class that spearates precesses of creating database and table related preproc classes
        /// </summary>
        public CenterDbPreprocFactory CenterDbPreprocFactory { get; private set; } 
        #endregion  // Properties

        #region Constructor
        /// <summary>
        /// Constructor of MainDbBranch
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
        /// Gets app layer database connection 
        /// </summary>
        public ICommonDbConnectionSV GetAppDbConnection() => DbConnectionPreproc.AppDbConnection; 
        /// <summary>
        /// Gets user layer database connection 
        /// </summary>
        public ICommonDbConnectionSV GetUserDbConnection() => DbConnectionPreproc.UserDbConnection; 
    }
}
