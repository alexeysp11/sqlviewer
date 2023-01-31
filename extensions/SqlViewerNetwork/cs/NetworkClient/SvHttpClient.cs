using System.Net.Http;
using System.Threading.Tasks;

namespace SqlViewerNetwork.NetworkClient
{
    public class SvHttpClient : INetworkClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task GetAsync(string requestUri)
        {
            try
            {
                //HttpResponseMessage response = await client.GetAsync(requestUri);
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();

                // Above three lines can be replaced with new helper method below
                string responseBody = await client.GetStringAsync(requestUri);
                // string responseBody = await (await client.GetAsync(requestUri)).EnsureSuccessStatusCode().Content.ReadAsStringAsync();

                System.Console.WriteLine(responseBody);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }
        
        public static void Get(string requestUri)
        {
            //Task task = GetAsync(requestUri);
            Task.Run(async () => await GetAsync(requestUri)).Wait();
        }
    }
}
