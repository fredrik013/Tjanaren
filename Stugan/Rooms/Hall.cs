using Stugan.Core;
using Stugan.Modeller.Saker;

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
                string text = "\n[SYSTEMMEDDELANDE: SMUTS-DETEKTOR AKTIVERAD]\n" +
                    "En gäll, metallisk röst ekar från taket:\n" +
                    "'STOPP! Obehörig smuts detekterad på Herrskapet Von Krångels parkett!'\n" +
                    "'Noll-tolerans råder. Sanering krävs omedelbart, tjänare!'\n\n" +
                    "Du känner hur nackhåren reser sig. Det här huset verkar ha ögon överallt.\n\n" +
                    "Tryck OK för att kliva in...";

                // Visa modal dialog i WinForms istället för Console.ReadKey
                MessageBox.Show(text, "Varning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Nu sätter vi flaggan så huset håller tyst nästa gång (men smutsen är kvar)
                story.HarBlivitUtskalldHallen = true;
            }

            return true;
        }


        public override string UndersokRum(Spelare spelare, Core.StoryState story)
        {
            string output = "";
            var plasttofflor = SakerIRummet.Find(s => s.Namn.ToLower().Contains("tofflor"));

            if (!HarUndersokts)
            {
                // Logiken är intakt: Vi gör tofflorna synliga
                if (plasttofflor != null) plasttofflor.ArGomd = false;

                output += "\nDu rotar bland de få sakerna i skohyllan...\n";
                output += "Där, längst in under en gammal tidning, hittar du ett par blå plasttofflor!\n";
                HarUndersokts = true;

                if (plasttofflor is Klader k && k.SkyddarMotVatten)
                {
                    output += "De ser ut att tåla både fukt och lort – perfekta för källaren.\n";
                }
            }
            else
            {
                output += "\nDu ser inget mer av värde i skohyllan, bara lite grus och dammråttor.\n";

                // Vi kollar aktivt skodon via spelaren
                if (spelare.AktivtSkodon.ToLower() == "ute")
                {
                    output += "Du ser att dina boots har lämnat leriga spår på det rena golvet. Aj då.\n";
                }
            }

            // HÄR ÄR RÄDDNINGEN: Vi returnerar hela strängen till spelmotorn
            return output;
        }
    }
}
