using SqlViewer.Models.AppBranches; 

namespace SqlViewer.ViewModels
{
    public class DataVM
    {
        public MainDbBranch MainDbBranch { get; private set; }
        public InterDbBranch InterDbBranch { get; private set; }
        public NetworkBranch NetworkBranch { get; private set; }

        public DataVM(MainVM mainVM)
        {
            this.MainDbBranch = new MainDbBranch(mainVM); 
            this.InterDbBranch = new InterDbBranch(); 
            this.NetworkBranch = new NetworkBranch(); 
        }
    }
}
