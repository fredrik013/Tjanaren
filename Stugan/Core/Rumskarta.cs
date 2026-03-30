using Stugan.Rooms;

namespace Stugan.Core
{
    public class Rumskarta
    {
        public WorldMap Karta { get; private set; }

        public StoryState Story { get; private set; }

        public Rumskarta()
        {
            Karta = new WorldMap();
            Story = new StoryState();
        }

        public static Rum SkapaVarlden(WorldMap karta, StoryState story)
        {
            Gardsplan gardsplan = new Gardsplan();
            gardsplan.Plats = new Position(0, 0, 0);
            karta.LaggTillRum(gardsplan.Plats, gardsplan);

            return gardsplan;
        }
    }
}
