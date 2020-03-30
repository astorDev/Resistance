using System;
using System.Threading.Tasks;
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
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHostedService, DiscordService>();
                    new Startup(hostContext.Configuration).ConfigureServices(services);
                });

            await builder.RunConsoleAsync();
        }
    }
}