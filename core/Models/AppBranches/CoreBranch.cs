namespace SqlViewer.Models.AppBranches
{
    /// <summary>
    /// ViewModel that deals with data 
    /// </summary>
    public class CoreBranch
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
        /// Constructor of CoreBranch
        /// </summary>
        public CoreBranch()
        {
            this.MainDbBranch = new MainDbBranch(); 
            this.InterDbBranch = new InterDbBranch(); 
        }
    }
}
