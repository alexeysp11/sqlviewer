using Microsoft.Extensions.Configuration; 

namespace SqlViewerNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Start program"); 
            IConfiguration config = new ConfigurationBuilder()
                .AddXmlFile("appsettings.xml")
                .AddEnvironmentVariables()
                .Build();
            
            Settings settings = config.GetSection("Settings").Get<Settings>();

            System.Console.WriteLine($"SomeSetting = {settings.SomeSetting}");
            System.Console.WriteLine($"Another = {settings.Another}");
            
            /*
            while (true)
            {
                try 
                {
                    // Operate 
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message); 
                    System.Console.WriteLine("Press Enter to continue"); 
                    System.Console.ReadLine(); 
                }
            }
            */
        }
    }
}
