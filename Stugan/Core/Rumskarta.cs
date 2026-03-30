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
            // Utomhus
            Gardsplan gardsplan = new Gardsplan();
            gardsplan.Plats = new Position(0, 0, 0);
            karta.LaggTillRum(gardsplan.Plats, gardsplan);

            Skjul skjul = new Skjul();
            skjul.Plats = new Position(1, 0, 0);
            karta.LaggTillRum(skjul.Plats, skjul);

            Tradgard tradgard = new Tradgard();
            tradgard.Plats = new Position(-2, 1, 0);
            karta.LaggTillRum(tradgard.Plats, tradgard);
            gardsplan.Koppla("Vaster", tradgard); // Pil Vänster på gården -> Trädgården
            tradgard.Koppla("Soder", gardsplan);  // Pil Höger i trädgården -> Gården

            // Entré
            Hall hall = new Hall();
            hall.Plats = new Position(0, 1, 0);
            karta.LaggTillRum(hall.Plats, hall);

            Kok kok = new Kok();
            kok.Plats = new Position(-1, 1, 0);
            karta.LaggTillRum(kok.Plats, kok);

            Omkladningsrum omkladningsrum = new Omkladningsrum();
            omkladningsrum.Plats = new Position(1, 1, 0);
            karta.LaggTillRum(omkladningsrum.Plats, omkladningsrum);

            return gardsplan;
        }
    }
}
