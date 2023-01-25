using System.Net; 
using System.Net.Sockets; 

namespace SqlViewerNetwork.NetworkServer
{
    public class TcpNetworkServer : INetworkServer
    {
        private TcpListener Listener { get; set; }  
        private TcpClient Client { get; set; } 

        public IPAddress Ip { get; }
        public string ServerName { get; private set; }
        private int Port { get; }

        private byte[] ReceivedBytes;
        private byte[] ResponseBytes; 

        public TcpNetworkServer()
        {
            this.Ip = IPAddress.Parse("127.0.0.1"); 
            this.ServerName = "localhost"; 
            this.Port = 4444; 

            this.StartServer(); 
        }

        public void StartServer()
        {
            this.Listener = new TcpListener(this.Ip, this.Port);
            this.Listen(); 
        } 
        public void StopServer()
        {
            this.Client.Close(); 
            this.Listener.Stop(); 
        } 

        public bool IsRunning()
        {
            return Listener != null; 
        }

        public void Listen()
        {
            try
            {
                this.Listener.Start();
                while(true)
                {
                    System.Console.Write("[OK] ");
                    GetMessage();
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        private void GetMessage()
        {
            ReceivedBytes = new byte[256]; 
            try
            {
                this.Client = this.Listener.AcceptTcpClient();
                NetworkStream stream = this.Client.GetStream();
                
                int msgLength = stream.Read(ReceivedBytes, 0, ReceivedBytes.Length); 
                
                ResponseBytes = System.Text.Encoding.ASCII.GetBytes("msg response"); 
                stream.Write(ResponseBytes, 0, ResponseBytes.Length); 
            }
            catch (System.Exception e)
            {
                throw e;
            }
            ReceivedBytes = new byte[1]; 
            ResponseBytes = new byte[1]; 
        }
    }
}
