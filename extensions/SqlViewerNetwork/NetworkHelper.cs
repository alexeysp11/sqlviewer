using SqlViewerNetwork.NetworkServer; 

namespace SqlViewerNetwork.Helpers
{
    public static class NetworkHelper
    {
        private static INetworkServer Server = null; 

        private static string ProtocolName; 
        private static string PrevProtocolName; 

        private static string SettingsFileString; 

        public static void InitServer()
        {
            try
            {
                SettingsFileString = GetSettingsFileString(); 
                PrevProtocolName = ProtocolName; 
                ProtocolName = GetProtocolName(); 
                if (Server == null)
                {
                    System.Console.Clear(); 
                    System.Console.WriteLine("SERVER STARTED");
                    System.Console.WriteLine("Protocol: " + ProtocolName);
                }
                else
                {
                    if (ProtocolName != PrevProtocolName) Server.StopServer(); 
                }
                Server = GetNetworkServer(); 
                if (!Server.IsRunning()) Server.StartServer(); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public static void Listen()
        {
            if (Server == null) throw new System.Exception("Server could not be null"); 
            try
            {
                Server.Listen(); 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        public static string GetSettingsFileString()
        {
            string path = GetSettingsFilePath(); 
            return System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path) : string.Empty; 
        }

        public static string GetProtocolName()
        {
            return SettingsFileString.ToUpper().Contains("http".ToUpper()) ? "HTTP" : "Unknown"; 
        }

        public static INetworkServer GetNetworkServer()
        {
            INetworkServer result; 
            result = new HttpNetworkServer(); 
            return ProtocolName != PrevProtocolName ? (result == null ? new HttpNetworkServer() : result) : Server; 
        }

        private static string GetSettingsFilePath()
        {
            return GetProjectDirectory() + "\\data\\SqlViewerNetwork\\settings.txt"; 
        }

        private static string GetProjectDirectory()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\.."; 
        }
    }
}
