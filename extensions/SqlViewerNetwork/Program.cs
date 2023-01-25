namespace SqlViewerNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try 
                {
                    SqlViewerNetwork.Helpers.NetworkHelper.InitServer(); 
                    SqlViewerNetwork.Helpers.NetworkHelper.Listen(); 
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message); 
                    System.Console.WriteLine("Press Enter to continue"); 
                    System.Console.ReadLine(); 
                }
            }
        }
    }
}
