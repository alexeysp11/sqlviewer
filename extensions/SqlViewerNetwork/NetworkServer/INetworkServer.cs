namespace SqlViewerNetwork.NetworkServer
{
    public interface INetworkServer
    {
        void StartServer(); 
        void StopServer(); 

        void Listen(); 
        bool IsRunning(); 
    }
}
