namespace SqlViewer.Models.NetworkPreproc
{
    public class GrpcPreproc : BaseNetworkPreproc, INetworkPreproc
    {
        public void StartServer()
        {
            base.InitNetworkExtension(this); 
        }
    }
}
