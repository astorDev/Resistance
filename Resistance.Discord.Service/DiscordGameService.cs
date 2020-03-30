using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Resistance.Domain;

namespace Resistance.Discord.Service
{
    public class DiscordGameService
    {
        public Game<DiscordPlayer> Game { get; }

        public DiscordGameService(Game<DiscordPlayer> game)
        {
            this.Game = game;
        }
        
        public async Task AcceptCrewAsync(MoveMessage message, DiscordSocketClient client)
        {
            this.Game.CurrentMission = new Mission<DiscordPlayer>(message.Raw.MentionedUsers.Select(u => new DiscordPlayer(u)));
            await this.Game.CurrentMission.StartAsync();
            await this.sendInGameChannelAsync("Команда набрана!", client);
        }

        public async Task SwearOnSpamAsync(MoveMessage message, DiscordSocketClient client)
        {
            await this.sendInGameChannelAsync($"{message.Raw.Author.Username}, засунь себе в жопу своё '{message.Raw.Content}'. Здесь пишут только составы мисиий", client);
        }
        
        public async Task AcceptVote(MoveMessage message, DiscordSocketClient client)
        {
            var vote = this.TryParse(message.Raw.Content);
            if (vote == null)
            {
                await message.Raw.Author.SendMessageAsync($"Читать научись! '{MoveMessage.Success}' или '{MoveMessage.Fail}', кусок говна!");
                return;
            }

            await this.Game.CurrentMission.AcceptVoteAsync(vote.Value, message.Raw.Author.Id.ToString());
            if (this.Game.CurrentMission.IsCompleted)
            {
                await this.sendInGameChannelAsync($"Миссия завершена. Провалов: {this.Game.CurrentMission.FailsCount}", client);
            }
        }

        public async Task HumiliateAsync(MoveMessage message)
        {
            await message.Raw.Channel.SendMessageAsync("С ботом решил поговорить лол. Совсем друзей, чтоли нет? Хотя не важно, не отвечай, даже мне не интересно");
        }

        private async Task sendInGameChannelAsync(string message, DiscordSocketClient client)
        {
            var gameChannel = (ITextChannel)client.GetChannel(DiscordService.GameChannelId);
            await gameChannel.SendMessageAsync(message);
        }

        private MissionVote? TryParse(string message)
        {
            switch (message)
            {
                case MoveMessage.Success : return MissionVote.Success;
                case MoveMessage.Fail : return MissionVote.Fail;
                default: return null;
            }
        }
    }
}