using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
{
    public class Hall : Rum
    {
        public Hall() : base("Hallen",
                    "Du kliver in i en hemtrevlig hall. Det doftar svagt av såpa och gammalt trä. " +
        "På väggen hänger en spegel och under den står en skohylla. " +
        "Till vänster ser du en dörr och till höger verkar det finnas en trappa mot källaren." + "Rakt fram fortsätter hallen längre in i huset.")
        {
            VisaNamn = "i hallen";
            UtgangsMeddelanden.Add("Ner", "Trappan gnisslar betänkligt under din vikt för varje steg du tar neråt...");
            OppnaUtgangar("Vaster", "Oster", "Norr", "Soder", "Ner");
            SakerIRummet.Add(new Klader("Plasttofflor", "Ett par blå plasttofflor.", Bekladnadstyp.Inne, Kroppsdel.Fot, BekladnadsLager.Mellan, true, true, true));
            // I Hall-konstruktorn:
            SakerIRummet.Add(new Inredning("Spegel", "En prålig spegel med guldram. Glaset är så rent att det nästan gör ont i ögonen. " +
                "Du ser en person som ser ut att ha sovit för lite och jobbat för mycket. En liten lortfläck på din kind lyser som en varningslampa – Herrskapet kommer få ett utbrott om de ser den."));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi kollar om spelaren har boots OCH om huset inte redan skällt ut dem
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString() && !story.HarBlivitUtskalldHallen)
            {
                WriteLine("\n[SYSTEMMEDDELANDE: SMUTS-DETEKTOR AKTIVERAD]");
                WriteLine("En gäll, metallisk röst ekar från taket:");
                WriteLine("'STOPP! Obehörig smuts detekterad på Herrskapet Von Krångels parkett!'");
                WriteLine("'Noll-tolerans råder. Sanering krävs omedelbart, tjänare!'");

                WriteLine("\nDu känner hur nackhåren reser sig. Det här huset verkar ha ögon överallt.");
                WriteLine("Tryck på valfri tangent för att kliva in...");
                ReadKey(true);

                // Nu sätter vi flaggan så huset håller tyst nästa gång (men smutsen är kvar)
                story.HarBlivitUtskalldHallen = true;
            }

            return true;
        }

        public override void UndersokRum(Spelare spelare, Core.StoryState story)
        {
            var rum = spelare.NuvarandeRum;
            var plasttofflor = SakerIRummet.Find(s => s.Namn.ToLower().Contains("tofflor"));

            if (!HarUndersokts)
            {
                plasttofflor.ArGomd = false;
                WriteLine("\nDu rotar bland de få sakerna i skohyllan...");
                WriteLine("Där, längst in under en gammal tidning, hittar du ett par blå plasttofflor!");
                HarUndersokts = true;

                // Här kan vi "skryta" lite med vår nya prop
                if (plasttofflor is Klader k && k.SkyddarMotVatten)
                {
                    WriteLine("De ser ut att tåla både fukt och lort – perfekta för källaren.");
                }
            }
            else
            {
                // En lite mer beskrivande text om man letar igen
                WriteLine("\nDu ser inget mer av värde i skohyllan, bara lite grus och dammråttor.");

                if (spelare.AktivtSkodon.ToLower() == "ute")
                {
                    WriteLine("Du ser att dina boots har lämnat leriga spår på det rena golvet. Aj då.");
                }
            }
        }
    }
}
