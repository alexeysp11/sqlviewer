namespace SqlViewer.Models.NetworkPreproc
{
    public class HttpPreproc : BaseNetworkPreproc, INetworkPreproc
    {
        public void StartServer()
        {
            base.InitNetworkExtension(this); 
        }
    }
}
