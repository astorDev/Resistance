namespace Resistance.Domain
{
    public class MissionParticipant<TPlayer>
    {
        public TPlayer Player { get; set; }
        
        public MissionVote? Vote { get; set; }

        public MissionParticipant(TPlayer player)
        {
            this.Player = player;
        }

        public bool HasVoted => this.Vote != null;
    }
}