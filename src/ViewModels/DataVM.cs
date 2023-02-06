using SqlViewer.Models.AppBranches; 

namespace SqlViewer.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DataVM
    {
        /// <summary>
        /// 
        /// </summary>
        public MainDbBranch MainDbBranch { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public InterDbBranch InterDbBranch { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DataVM(MainVM mainVM)
        {
            this.MainDbBranch = new MainDbBranch(mainVM); 
            this.InterDbBranch = new InterDbBranch(); 
        }
    }
}
