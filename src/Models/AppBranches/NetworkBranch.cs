using System.Net.NetworkInformation; 
using System.Net.Sockets; 
using SqlViewer.Models.DbTransfer; 
using SqlViewer.Models.NetworkPreproc; 

namespace SqlViewer.Models.AppBranches
{
    public class NetworkBranch
    {
        public INetworkPreproc CommunicationNetworkPreproc { get; private set; }
        public INetworkPreproc TransferNetworkPreproc { get; private set; }

        public DbInterconnection DbInterconnection { get; private set; }

        public NetworkBranch()
        {
            this.DbInterconnection = new DbInterconnection(); 
        }

        private bool IsDifferentNetworkPreproc(INetworkPreproc networkPreproc, string typeName) 
        {
            return string.IsNullOrEmpty(typeName) || networkPreproc == null || !networkPreproc.GetType().ToString().Contains(typeName);
        }

        public void InitCommunicationProtocol(string protocolName)
        {
            if (string.IsNullOrEmpty(protocolName))
                throw new System.Exception("Protocol name 'protocolName' could not be null or empty"); 
            
            try
            {
                switch (protocolName)
                {
                    case "HTTP":
                        if (IsDifferentNetworkPreproc(this.CommunicationNetworkPreproc, "HttpPreproc")) 
                            this.CommunicationNetworkPreproc = new HttpPreproc(); 
                        break;

                    case "TCP":
                        if (IsDifferentNetworkPreproc(this.CommunicationNetworkPreproc, "TcpPreproc")) 
                            this.CommunicationNetworkPreproc = new TcpPreproc(); 
                        break;

                    case "SOAP":
                        if (IsDifferentNetworkPreproc(this.CommunicationNetworkPreproc, "SoapPreproc")) 
                            this.CommunicationNetworkPreproc = new SoapPreproc(); 
                        break;

                    case "REST API":
                        if (IsDifferentNetworkPreproc(this.CommunicationNetworkPreproc, "RestApiPreproc")) 
                            this.CommunicationNetworkPreproc = new RestApiPreproc(); 
                        break;

                    case "gRPC":
                        if (IsDifferentNetworkPreproc(this.CommunicationNetworkPreproc, "GrpcPreproc")) 
                            this.CommunicationNetworkPreproc = new GrpcPreproc(); 
                        break;

                    default: 
                        throw new System.Exception("Incorrect ProtocolName: '" + protocolName + "'"); 
                        break; 
                }
                this.CommunicationNetworkPreproc.StartServer(); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        public void InitTransferProtocol(string protocolName)
        {
            if (string.IsNullOrEmpty(protocolName))
                throw new System.Exception("Protocol name 'protocolName' could not be null or empty"); 
            
            switch (protocolName)
            {
                case "HTTP":
                    if (IsDifferentNetworkPreproc(this.TransferNetworkPreproc, "HttpPreproc")) 
                            this.TransferNetworkPreproc = new HttpPreproc(); 
                    break;

                default: 
                    throw new System.Exception("Incorrect ProtocolName: '" + protocolName + "'"); 
                    break; 
            }
        }

        public string GetLocalIp()
        {
            int port = 8000; 
            string ipEthernet = GetLocalIPv4(NetworkInterfaceType.Ethernet); 
            string ipWireless = GetLocalIPv4(NetworkInterfaceType.Wireless80211); 
            return "Ethernet: " + ipEthernet + ":" + port + ", Wireless: " + ipWireless + ":" + port; 
        }
        private string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return string.IsNullOrEmpty(output) ? "no connection" : output;
        }

        public bool PingHost(string nameOrAddress)
        {
            if (string.IsNullOrEmpty(nameOrAddress))
                throw new System.Exception("Host could not be null or empty"); 
            
            bool pingable = false;
            Ping pinger = null;
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }
    }
}
