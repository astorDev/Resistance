using System.Threading.Tasks;

namespace Resistance.Domain
{
    public abstract class Player
    {
        public string Id { get; set; }

        public abstract Task StartMissionAsync();

        public abstract Task AcceptMissionVoteGratitudeAsync();
    }
}