namespace SqlViewer.Models.NetworkPreproc
{
    public class SoapPreproc : BaseNetworkPreproc, INetworkPreproc
    {
        public void StartServer()
        {
            base.InitNetworkExtension(this); 
        }
    }
}
