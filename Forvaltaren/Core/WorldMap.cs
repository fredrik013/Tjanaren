using Forvaltaren.Rooms;

namespace Forvaltaren.Core
{
    public class WorldMap
    {
        // En ordbok som kopplar en koordinat till ett specifikt rum
        private readonly Dictionary<Position, Rum> _rooms = new Dictionary<Position, Rum>();

        // Metod för att lägga till rum på kartan
        public void LaggTillRum(Position pos, Rum rum)
        {
            if (!_rooms.ContainsKey(pos))
            {
                _rooms.Add(pos, rum);
            }
        }

        // Metod för att hämta ett rum baserat på koordinater
        public Rum HamtaRum(Position pos)
        {
            if (_rooms.TryGetValue(pos, out var rum))
            {
                return rum;
            }
            return null; // Inget rum på denna position
        }
    }
}