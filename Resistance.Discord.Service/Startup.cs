using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resistance.Domain;

namespace Resistance.Discord.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<Game<DiscordPlayer>>();
            services.AddSingleton<DiscordGameService>();
        }
    }
}