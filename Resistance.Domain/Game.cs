using System.Threading.Tasks;

namespace Resistance.Domain
{
    public class Game<TPlayer> where TPlayer : Player
    {
        public Mission<TPlayer> CurrentMission { get; set; }
    }
}