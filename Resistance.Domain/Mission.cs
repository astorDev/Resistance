using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resistance.Domain
{
    public class Mission<TPlayer> where TPlayer : Player
    {
        public MissionParticipant<TPlayer>[] Crew { get; }

        public bool IsCompleted => this.Crew.All(m => m.HasVoted);
        
        public Mission(IEnumerable<TPlayer> crew)
        {
            this.Crew = crew.Select(p => new MissionParticipant<TPlayer>(p)).ToArray();
        }

        public int FailsCount => this.Crew.Count(m => m.Vote == MissionVote.Fail);
        
        public async Task StartAsync()
        {
            foreach (var member in this.Crew)
            {
                await member.Player.StartMissionAsync();
            }
        }

        public bool HasParticipantWhoHasNotVoted(string id)
        {
            return this.Crew.Any(p => p.Player.Id == id && !p.HasVoted);
        }

        public async Task AcceptVoteAsync(MissionVote vote, string playerId)
        {
            var member = this.Crew.Single(m => m.Player.Id == playerId);
            member.Vote = vote;
            await member.Player.AcceptMissionVoteGratitudeAsync();
        }
    }
}