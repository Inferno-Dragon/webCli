
using System;
using System.IO;
using System.Net.Http;

namespace webCLI
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)

        {
            // Check to see if the lenth of args is bigger than zero
            if (args.Length <= 0)
            {
                throw new System.InvalidOperationException("No args passed in the CLI");
            }

            InputDto myObj = new InputDto();

            // Looped through the array using a advanced for loop
            for (int i = 0; i < args.Length; i++)
            {

                if (args[i] == "--uri")
                {
                    myObj.Uri = args[i + 1];

                }

                if (args[i] == "--output")
                {
                    myObj.Output = args[i + 1];
                }
            }

            // Printing parameters and hello world
            Console.WriteLine("Hello World!");
            Console.WriteLine(myObj.Uri);
            Console.WriteLine(myObj.Output);

            // Check if uri is valid
            if (Uri.IsWellFormedUriString(myObj.Uri, UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("Uri is valid");
            }
            else
            {
                throw new System.InvalidOperationException("Uri is not valid");
            }



            // Check if file path is correct
            if (Path.IsPathFullyQualified(myObj.Output))
            {
                Console.WriteLine("File is valid ");
            }

            else
            {
                throw new System.InvalidOperationException("File is not valid");
               
            }

            // Throw exception if both the parameters are empty
            if (myObj.Uri == " " || myObj.Output == " ")
            {

                throw new System.InvalidOperationException("Both parameters are empty or one is empty");
            }

            // Initialised HttpClient
            HttpClient client = new HttpClient();

            string responseBody;

            // Making web request with uri 
            try
            {
                HttpResponseMessage response = await client.GetAsync(myObj.Uri);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseBody);

                // If file exists overwrite
                if (File.Exists(myObj.Output))
                    File.AppendAllText(myObj.Output, responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
