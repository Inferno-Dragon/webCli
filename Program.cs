using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;


namespace webCLI
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)

        {
            // Implemented configurator
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddCommandLine(args);

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

            InputDto inputs = new InputDto();

            inputs.Uri = configuration.GetSection("uri").Value;
            inputs.Output = configuration.GetSection("output").Value;

            // Printing parameters and hello world
            logger.LogInformation("Hello World!");
            logger.LogInformation(inputs.Uri);
            logger.LogInformation(inputs.Output);

            Core.IsValidUri(inputs.Uri);

            Core.IsValidOutput(inputs.Output);

            await MyHttpRequest.MakeHttpRequest(inputs.Uri, inputs.Output, logger);

        }
    }
}
