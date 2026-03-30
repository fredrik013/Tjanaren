using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;
namespace Stugan.Rooms
{
    public class InreHall : Rum
    {
        public InreHall() : base("Hallen", "Du fortsätter längre in i hallen. Golvet här täcks av en tjock, ljus och extremt mjuk heltäckningsmatta " +
            "som verkar suga upp allt ljud. Till vänster hörs det dämpade ljudet från en TV – det måste vara vardagsrummet.")
        {
            VisaNamn = "i hallen";
            OppnaUtgangar("Soder", "Vaster");

            SakerIRummet.Add(new AllmanSak("Nyckel", "En rostig gammal nyckel. Undrar var den leder.", true, true)
            { ArAktiv = true });
        }

        public override void UndersokRum(Spelare spelare, Core.StoryState story)
        {
            base.UndersokRum(spelare, story);
            var rum = spelare.NuvarandeRum;
            var nyckel = SakerIRummet.Find(s => s.Namn.ToLower().Contains("nyckel"));

            if (string.IsNullOrEmpty(spelare.AktivtSkodon) && !HarUndersokts)
            {
                WriteLine("\nDen mjuka mattan kittlar skönt mellan dina bara tår. Plötsligt känner du något hårt.");
                WriteLine("Du lyfter på mattkanten och hittar en rostig nyckel!");
                nyckel.ArGomd = false;
                story.HarNyckel = true;
                HarUndersokts = true;
                return; // Vi går ur här så vi inte får dubbla meddelanden från switchen
            }

            // Vi switchar på typen, men hämtar namnet dynamiskt för texten
            switch (spelare.AktivtSkodon.ToLower())
            {
                case "ute":
                    WriteLine("\n[VARNING]");
                    // Vi använder spelarens aktiva skodon-namn istället för "Boots"
                    WriteLine($"Mörka lerfläckar från dina {spelare.AktivtSkodonNamn} breder ut sig på den ljusa mattan.");
                    WriteLine("En röst viskar strängt: 'Vissa skor hör hemma på bron, inte på finmattan...'");
                    break;

                case "inne":
                    WriteLine($"\nDina {spelare.AktivtSkodonNamn} glider ljudlöst över den mjuka mattan.");
                    WriteLine($"Du förnimmer något under foten men sulorna på dina {spelare.AktivtSkodonNamn} är för tjocka för att du ska kunna veta vad det är.");
                    break;

                case "":
                    WriteLine("\nDen mjuka mattan kittlar skönt mellan dina bara tår.");
                    break;
            }


        }
    }
}
