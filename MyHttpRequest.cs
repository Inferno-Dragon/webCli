using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace webCLI
{
    public class MyHttpRequest
    {
        public static async Task MakeHttpRequest(string url, string filePath, ILogger logger)
        {

            // Initialised HttpClient
            HttpClient client = new HttpClient();

            // Making web request with uri 
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                logger.LogInformation(responseBody);

                File.WriteAllText(filePath, responseBody);

            }
            catch (HttpRequestException e)
            {
                logger.LogInformation("\nException Caught!");
                logger.LogInformation("Message :{0} ", e.Message);
            }
        }
    }
}
