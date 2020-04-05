using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Resistance.Discord.Service
{
    public class DiscordService : IHostedService
    {
        public const long GameChannelId = 694523410700566529;
        public DiscordSocketClient Client { get; }
        public DiscordGameService GameService { get; }
        public IConfiguration Configuration { get; }

        public DiscordService(DiscordSocketClient client, DiscordGameService gameService, IConfiguration configuration)
        {
            this.Client = client;
            this.GameService = gameService;
            this.Configuration = configuration;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.Client.Log += async message => Console.WriteLine(message.Message);
            this.Client.MessageReceived += async message =>
            {
                try
                {
                    if (message.Author.Username == "ResistanceBot")
                    {
                        return;
                    }

                    var dm = await this.Client.GetDMChannelAsync(message.Channel.Id);
                    if (message.Channel.Id != GameChannelId && dm == null)
                    {
                        return;
                    }
                    
                    var move = MoveMessage.Interpret(message, this.GameService.Game);
                    switch (move.Type)
                    {
                        case MoveMessage.MoveType.CrewDeclaration:
                            await this.GameService.AcceptCrewAsync(move, this.Client);
                            break;
                        case MoveMessage.MoveType.MissionVote:
                            await this.GameService.AcceptVote(move, this.Client);
                            break;
                        case MoveMessage.MoveType.ChatSpam:
                            await this.GameService.SwearOnSpamAsync(move, this.Client);
                            break;
                        case MoveMessage.MoveType.TalkingToBot:
                            await this.GameService.HumiliateAsync(move);
                            break;
                        default:
                            throw new NotImplementedException("unknown message type");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };
            
            await this.Client.LoginAsync(TokenType.Bot, this.Configuration["BotToken"]);

            await this.Client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}