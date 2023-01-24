using SqlViewerNetwork.NetworkServer; 

namespace SqlViewerNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Clear(); 
            System.Console.WriteLine("SERVER STARTED");

            string protocolName = "HTTP"; 
            System.Console.WriteLine("Protocol: " + protocolName);

            INetworkServer server = new HttpNetworkServer(); 
            server.StartServer(); 
            while (true)
            {
                string str = System.Console.ReadLine(); 
                if (str == "kill")
                    server.StopServer(); 
            }
        }
    }
}
