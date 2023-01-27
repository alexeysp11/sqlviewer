namespace SqlViewer.Models.NetworkPreproc
{
    public class TcpPreproc : BaseNetworkPreproc, INetworkPreproc
    {
        public void StartServer()
        {
            base.InitNetworkExtension(this); 
        }
    }
}
