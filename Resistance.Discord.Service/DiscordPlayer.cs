using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Resistance.Discord.Service
{
    public class DiscordPlayer : Domain.Player
    {
        public SocketUser User { get; }

        public DiscordPlayer(SocketUser user)
        {
            this.User = user;
            this.Id = user.Id.ToString();
        }

        public override async Task StartMissionAsync()
        {
            await this.User.SendMessageAsync($"Тебя взяли в миссиию. Выбери '{MoveMessage.Success}' или '{MoveMessage.Fail}'");
        }

        public override async Task AcceptMissionVoteGratitudeAsync()
        {
            await this.User.SendMessageAsync("Красава!");
        }
    }
}