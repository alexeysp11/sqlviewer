namespace SqlViewer.Models.NetworkPreproc
{
    public class RestApiPreproc : BaseNetworkPreproc, INetworkPreproc
    {
        public void StartServer()
        {
            base.InitNetworkExtension(this); 
        }
    }
}
