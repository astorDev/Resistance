using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Resistance.Discord.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var builder = new HostBuilder()
                              .ConfigureAppConfiguration((context, config) =>
                              {
                                  var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                  var settingsPath = $"{userFolder}/SecretSettings/resistance.Production.json";
                                  config.AddJsonFile(settingsPath);
                              })
                              .ConfigureServices((hostContext, services) =>
                              {
                                  services.AddSingleton<IHostedService, DiscordService>();
                                  new Startup(hostContext.Configuration).ConfigureServices(services);
                              });

                await builder.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}