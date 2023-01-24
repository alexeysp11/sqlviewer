using SqlViewer.Models.AppClients; 

namespace SqlViewer.ViewModels
{
    public class DataVM
    {
        public MainDbClient MainDbClient { get; private set; }
        public InterDbClient InterDbClient { get; private set; }
        public NetworkClient NetworkClient { get; private set; }

        public DataVM(MainVM mainVM)
        {
            this.MainDbClient = new MainDbClient(mainVM); 
            this.InterDbClient = new InterDbClient(); 
            this.NetworkClient = new NetworkClient(); 
        }
    }
}
