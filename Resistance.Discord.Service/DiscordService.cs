using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Resistance.Discord.Service
{
    public class DiscordService : IHostedService
    {
        public const long GameChannelId = 694219121214357578;
        
        public DiscordSocketClient Client { get; }
        public DiscordGameService GameService { get; }

        public DiscordService(DiscordSocketClient client, DiscordGameService gameService)
        {
            this.Client = client;
            this.GameService = gameService;
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

            
            
            await this.Client.LoginAsync(TokenType.Bot,
                "Njk0MTQ3MTgzMjc2MTk1ODcx.XoIcpA.XdY3T_mQ6dgYfi4Z8dMBJDdVIHk");

            var gcs = this.Client.GetChannel(3);
            
            await this.Client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}