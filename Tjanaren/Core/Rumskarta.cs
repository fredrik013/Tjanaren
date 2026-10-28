using Tjanaren.Rooms;

namespace Tjanaren.Core
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

            // Inre regionen
            Glasveranda glasveranda = new Glasveranda();
            glasveranda.Plats = new Position(-2, 2, 0);
            karta.LaggTillRum(glasveranda.Plats, glasveranda);

            InreHall hall2 = new InreHall();
            hall2.Plats = new Position(0, 2, 0);
            karta.LaggTillRum(hall2.Plats, hall2);

            Vardagsrum vardagsrum = new Vardagsrum();
            vardagsrum.Plats = new Position(-1, 2, 0);
            karta.LaggTillRum(vardagsrum.Plats, vardagsrum);

            // Källare
            Kallare kallare = new Kallare();
            kallare.Plats = new Position(0, 1, -1);
            karta.LaggTillRum(kallare.Plats, kallare);

            Matkallare matkallare = new Matkallare();
            matkallare.Plats = new Position(0, 2, -1);
            karta.LaggTillRum(matkallare.Plats, matkallare);

            Pannrum pannrum = new Pannrum();
            pannrum.Plats = new Position(1, 1, -1);
            karta.LaggTillRum(pannrum.Plats, pannrum);

            Tjanarrum tjanarrum = new Tjanarrum();
            tjanarrum.Plats = new Position(0, 0, -1);
            karta.LaggTillRum(tjanarrum.Plats, tjanarrum);

            Tvattstuga tvattstuga = new Tvattstuga(karta);
            tvattstuga.Plats = new Position(-1, 1, -1);
            karta.LaggTillRum(tvattstuga.Plats, tvattstuga);

            Stadskrubb stadskrubb = new Stadskrubb();
            stadskrubb.Plats = new Position(-2, 1, -1);
            karta.LaggTillRum(stadskrubb.Plats, stadskrubb);

            return gardsplan;
        }
    }
}
