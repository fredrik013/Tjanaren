using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
{
    public class Tradgard : Rum
    {
        public Tradgard() : base("Trädgården",
            "Du kliver ut i den vildvuxna trädgården. Gräset är högt och fuktigt, " +
            "och doften av våt jord är stark. Här och var ser man spår av gamla odlingar. Åt sydost skymtar man en grind mot gården.")
        {
            VisaNamn = "i trädgården";
            UtgangsMeddelanden.Add("Soder", "Du följer husväggen söderut och kommer in på en stig som leder fram till grinden.");
            OppnaUtgangar("Soder", "Norr");
            SakerIRummet.Add(new Redskap("Rörtång", "En tung och lite rostig rörtång. Perfekt för trilskande rör.", true, true));
        }

        public override void UndersokRum(Spelare spelare, StoryState story)
        {
            var rortang = SakerIRummet.FirstOrDefault(s => s.Namn == "Rörtång");

            if (rortang != null && rortang.ArGomd)
            {
                // HÄR ÄR FIXEN: Vi kollar om spelaren har en AKTIV spade
                var aktivSpade = spelare.Ryggsack.GetAllaSaker()
                    .OfType<Redskap>()
                    .FirstOrDefault(r => r.Namn == "Spade" && r.ArAktiv);

                if (aktivSpade != null)
                {
                    rortang.ArGomd = false;
                    story.HarHittatRortang = true;
                    WriteLine("\nDu greppar tag i spaden och sätter bladet i den mjuka jorden.");
                    WriteLine("Klonk! Spaden träffar något hårt. Du drar upp en rostig rörtång!");
                }
                else
                {
                    // Vi "jävlas" lite genom att ge olika ledtrådar
                    bool harSpadeMenInaktiv = spelare.Ryggsack.GetAllaSaker().Any(s => s.Namn == "Spade");

                    if (harSpadeMenInaktiv)
                    {
                        WriteLine("\nDet ser ut som att någon har grävt här tidigare. Markytan är lös.");
                        WriteLine("Du provar att krafsa lite med fingrarna, men det är lönlöst.");
                        WriteLine("Du behöver nog ta fram ett riktigt redskap om du ska komma någon vart.");
                    }
                    else
                    {
                        WriteLine("\nDu rotar runt i gräset. Marken känns mjuk och uppbökad på ett ställe.");
                        WriteLine("Det verkar finnas något här under, men du behöver nog ett verktyg för att få upp det.");
                    }
                }
            }
            else
            {
                WriteLine("\nDu ser inget annat än blött gräs och lera här ute.");
            }
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi matchar mot Enum-namnen (utan ToLower för att vara konsekventa)
            switch (s.AktivtSkodon)
            {
                case var skodon when skodon == Bekladnadstyp.Inne.ToString():
                    WriteLine("\n[EN STRÄNG RÖST FRÅN VERANDAN]");
                    WriteLine("'Men vad sysslar du med?! Gå inte ut här och skita ner dina tofflor!'");
                    WriteLine("'Visa lite hyfs och byt om till något som tål lera innan du sätter din fot på gräsmattan.'");

                    // Bra JAWS-fix! Vi behåller den.
                    System.Threading.Thread.Sleep(2000);
                    WriteLine("\n(Tryck på en tangent för att backa...)");
                    ReadKey(true);

                    return false;

                case var skodon when skodon == Bekladnadstyp.Ute.ToString():
                    WriteLine("\nDet klafsar till när bootsen möter den mjuka jorden.");
                    WriteLine("Du ser hur sulorna omedelbart täcks av lera.");
                    return true;

                case var skodon when skodon == Bekladnadstyp.Skydd.ToString():
                    WriteLine("\nGummistövlarna klafsar tryggt i leran. Det här är deras rätta element.");
                    return true;

                default:
                    WriteLine("\nDu kliver ut barfota i det våta gräset. Det är kallt och klibbigt.");
                    return true;
            }
        }
    }
}