using Microsoft.Extensions.Configuration; 

namespace SqlViewerConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Settings settings = config.GetSection("Settings").Get<Settings>();

            System.Console.WriteLine($"KeyOne = {settings.KeyOne}");
            System.Console.WriteLine($"KeyTwo = {settings.KeyTwo}");
            System.Console.WriteLine($"KeyThree:Message = {settings.KeyThree.Message}");
        }
    }
}
