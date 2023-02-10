using SqlViewer.Models.AppBranches; 

namespace SqlViewer.ViewModels
{
    /// <summary>
    /// ViewModel that deals with data 
    /// </summary>
    public class DataVM
    {
        /// <summary>
        /// Main branch of using DB functionality in the application 
        /// </summary>
        public MainDbBranch MainDbBranch { get; private set; }
        /// <summary>
        /// Branch of using DB functionality in the application, that connects two different databases  
        /// </summary>
        public InterDbBranch InterDbBranch { get; private set; }

        /// <summary>
        /// Constructor of DataVM
        /// </summary>
        public DataVM(MainVM mainVM)
        {
            this.MainDbBranch = new MainDbBranch(mainVM); 
            this.InterDbBranch = new InterDbBranch(); 
        }
    }
}
