using System.Linq;
using Discord;
using Discord.WebSocket;
using Resistance.Domain;

namespace Resistance.Discord.Service
{
    public class MoveMessage
    {
        public const string Success = "успех";

        public const string Fail = "провал";
        
        public SocketMessage Raw { get; }
        public MoveType Type { get; }

        public MoveMessage(SocketMessage raw, MoveType type)
        {
            this.Raw = raw;
            this.Type = type;
        }

        public static MoveMessage Interpret(SocketMessage message, Game<DiscordPlayer> game)
        {
            if (message.Channel.Id == DiscordService.GameChannelId)
            {
                return message.MentionedUsers.Any() ? new MoveMessage(message, MoveType.CrewDeclaration) : new MoveMessage(message, MoveType.ChatSpam);
            }
            
            if (game.CurrentMission != null && game.CurrentMission.HasParticipant(message.Author.Id.ToString()))
            {
                return new MoveMessage(message, MoveType.MissionVote);
            }
            
            return new MoveMessage(message, MoveType.TalkingToBot);
        }
        
        public enum MoveType
        {
            CrewDeclaration,
            
            MissionVote,
            
            ChatSpam,
            
            TalkingToBot
        }
    }
}