using SqlViewer.ViewModels; 

namespace SqlViewer.Models.Creational.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class CenterDbPreprocFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public DatabasePreprocFactory DatabasePreprocFactory { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public TablePreprocFactory TablePreprocFactory { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public CenterDbPreprocFactory(MainVM mainVM)
        {
            DatabasePreprocFactory = new DatabasePreprocFactory(mainVM);
            TablePreprocFactory = new TablePreprocFactory(mainVM);
        }
    }
}
