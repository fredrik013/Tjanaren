using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class InreHall : Rum
    {
        private readonly WorldMap _karta;
        public InreHall(WorldMap karta) : base("Hallen", "Du fortsätter längre in i hallen. Golvet här täcks av en tjock, ljus och extremt mjuk heltäckningsmatta " +
            "som verkar suga upp allt ljud. Till vänster hörs det dämpade ljudet från en TV – det måste vara vardagsrummet. " + "Till höger ser du en stängd dörr av snidad ek. Antagligen leder den till ett privat rum. " + "Längre in i hallen finns en trappa som leder till övervåningen.")
        {
            _karta = karta;
            VisaNamn = "i hallen";
            OppnaUtgangar("Soder", "Vaster", "Oster", "Upp");

            SakerIRummet.Add(new AllmanSak("Nyckel", "En rostig gammal nyckel. Undrar var den leder.", true, true)
            { ArAktiv = true });
            SakerIRummet.Add(new Inredning("Ekdörr", "Du undersöker den snidade ekdörren. Förutom det gedigna mässingshandtaget upptäcker du ett kraftigt lås. Du låter fingrarna följa de omsorgsfullt utskurna mönstren i träet.\r\n\r\nPlötsligt hörs en barsk röst från andra sidan dörren:\r\n\r\n\"Vem är det som står och tafsar på min dörr?! Har tjänstefolket verkligen inget bättre för sig?\"", false, false));
        }

        public override string UndersokRum(Spelare spelare, StoryState story)
        {
            // Vi skapar vår textbehållare
            string output = "";
            if (story.HallenArSkurad)
            {
                return "\nDu ser dig omkring i den välstädade hallen. " +
                       "Den ljusa heltäckningsmattan är nu ren och fri från smutsiga fotspår.\n";
            }

            // Vi hämtar bas-beskrivningen om du vill ha med den (valfritt)
            // output += base.UndersokRum(spelare, story) + "\n";

            var nyckel = SakerIRummet.Find(s => s.Namn.ToLower().Contains("nyckel"));

            // Logik för att hitta nyckeln barfota
            if (string.IsNullOrEmpty(spelare.AktivtSkodonNamn) && !HarUndersokts)
            {
                output += "\nDen mjuka mattan kittlar skönt mellan dina bara tår. Plötsligt känner du något hårt.\n";
                output += "Du lyfter på mattkanten och hittar en rostig nyckel!\n";

                if (nyckel != null) nyckel.ArGomd = false;
                story.HarNyckel = true;
                HarUndersokts = true;

                return output; // Vi returnerar direkt här, precis som din gamla kod gjorde
            }

            // Switch-satsen för olika skodon
            // Vi kollar mot AktivtSkodonNamn eller typen
            switch (spelare.AktivtSkodon.ToLower())
            {
                case "ute":
                    output += "\n[VARNING]\n";
                    output += $"Mörka lerfläckar från dina {spelare.AktivtSkodonNamn} breder ut sig på den ljusa mattan.\n";
                    output += "En röst viskar strängt: 'Vissa skor hör hemma på bron, inte på finmattan...'\n";
                    break;

                case "inne":
                    output += $"\nDina {spelare.AktivtSkodonNamn} glider ljudlöst över den mjuka mattan.\n";
                    output += $"Du förnimmer något under foten men sulorna på dina {spelare.AktivtSkodonNamn} är för tjocka för att du ska kunna veta vad det är.\n";
                    break;

                default: // Täcker in både barfota och annat
                    output += "\nDen mjuka mattan kittlar skönt mellan dina bara tår.\n";
                    break;
            }

            return output;
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            var mopp = spelare.Ryggsack.GetAllaSaker()
     .OfType<Redskap>()
     .FirstOrDefault(r => r.Namn == "Mopp" && r.ArAktiv);

            var dammsugare = spelare.Ryggsack.GetAllaSaker()
.OfType<Redskap>()
.FirstOrDefault(r => r.Namn == "Dammsugare" && r.ArAktiv);

            var hink = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>()
                .FirstOrDefault(r => r.Namn == "Hink" && r.ArAktiv);

            var vattentaligaSkor = spelare.Ryggsack.GetAllaSaker()
    .OfType<Klader>()
    .FirstOrDefault(k => k.ArAktiv && k.SkyddarMotVatten);

            switch (true)
            {
                case true when story.HallenArSkurad:
                    return "\nDu ser dig omkring i den skurade hallen. Det är inte värt att smutsa ner mattan.";

                case true when !story.SkurvattenKlart:
                    return "\nDu ser dig omkring. Det här kräver en ordentlig rengöring, men du har ännu inte gjort alla förberedelser.";

                case true when vattentaligaSkor == null:
                    return "\nDu behöver lämpliga skor som tål vatten för det här uppdraget.";

                case true when hink == null:
                    return "\nDu behöver något att ha rengöringsvattnet i. Se också till att ha utrustningen framme när du börjar.";

                case true when mopp == null:
                    return "\nDet här golvet blir inte rent av sig självt. Du behöver något lämpligt att rengöra det med, och du måste ha det framme.";

                case true when dammsugare == null:
                    return "\nDet här golvet blir inte rent av sig självt. Du behöver något lämpligt att rengöra det med, och du måste ha det framme.";

                default:
                    story.HallenArSkurad = true;
                    mopp.ArRen = false;
                    if (_karta.HamtaRum(new Position(-2, 1, -1))
                        is Stadskrubb stadskrubb)
                    {
                        spelare.NuvarandeRum = stadskrubb;
                        spelare.Ryggsack.GetAllaSaker().Remove(dammsugare);
                        stadskrubb.SakerIRummet.Add(dammsugare);
                    }
                    return "\nDu tar itu med det omfattande städarbetet. " +
                        "Du börjar med att dammsuga hela hallen noga. Sedan tar du itu med att skura hallen. " +
                        "Det smutsiga vattnet skvalpar medan du arbetar dig fram över hallgolvet.\n" +
                           "Efter ett rejält arbetspass är hallen  äntligen helt ren.\n" +
                           "Du betraktar resultatet med tillfredsställelse. " +
                            "Moppen däremot har sett betydligt bättre dagar. " +
                           "Med en lättad suck släpar du ner redskapen till städskrubben.\n\n" +
                           spelare.NuvarandeRum.VisaBeskrivning();

            }
        }
    }
}
