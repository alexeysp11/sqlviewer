using SqlViewer.Models.AppBranches; 

namespace SqlViewer.Models.Creational.DbPreproc
{
    /// <summary>
    /// Allows to spearate precesses of creating database and table related preproc classes
    /// </summary>
    public class CenterDbPreprocFactory
    {
        /// <summary>
        /// Class that creates database related preproc classes 
        /// </summary>
        public DatabasePreprocFactory DatabasePreprocFactory { get; private set; } 
        /// <summary>
        /// Class that creates table related preproc classes 
        /// </summary>
        public TablePreprocFactory TablePreprocFactory { get; private set; } 

        /// <summary>
        /// Constructor of CenterDbPreprocFactory
        /// </summary>
        public CenterDbPreprocFactory(MainDbBranch mainDbBranch)
        {
            DatabasePreprocFactory = new DatabasePreprocFactory(mainDbBranch);
            TablePreprocFactory = new TablePreprocFactory(mainDbBranch);
        }
    }
}
