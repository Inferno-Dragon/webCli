using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;

namespace webCLI
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)

        {
            // Implemented configurator
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // Implemented Logger
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();

            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Program started");

            InputDto myObj = new InputDto();
            
            // Check to see if the lenth of args is bigger than zero
            if (args.Length <= 0)
            {
                myObj.Uri = configuration.GetSection("settings:uri").Value;
                myObj.Output = configuration.GetSection("settings:output").Value;
            }
            else
            {
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
            }

            // Printing parameters and hello world
            logger.LogInformation("Hello World!");
            logger.LogInformation(myObj.Uri);
            logger.LogInformation(myObj.Output);

            // Check if uri is valid
            if (Uri.IsWellFormedUriString(myObj.Uri, UriKind.RelativeOrAbsolute))
            {
                logger.LogInformation("Uri is valid");
            }
            else
            {
                throw new System.InvalidOperationException("Uri is not valid");
            }

            // Check if file path is correct
            if (Path.IsPathFullyQualified(myObj.Output))
            {
                logger.LogInformation("File name entered is valid ");
            }

            else
            {
                throw new System.InvalidOperationException("File name is not valid");

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

                logger.LogInformation(responseBody);

                File.WriteAllText(myObj.Output, responseBody);

            }
            catch (HttpRequestException e)
            {
                logger.LogInformation("\nException Caught!");
                logger.LogInformation("Message :{0} ", e.Message);
            }

        }


    }
}
