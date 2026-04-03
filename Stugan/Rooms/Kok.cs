using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan.Rooms
{
    public class Kok : Rum
    {
        public Kok() : base("Köket",
            "Du kommer in i ett ljust och rymligt kök. Här doftar det av nybakat bröd och en hint av humle. " +
            "Köksbordet står dukat vid fönstret.")
        {
            VisaNamn = "i köket";
            OppnaUtgangar("Oster");
            SakerIRummet.Add(new Livsmedel("Eriksberg", "En immande kall Eriksberg Karaktär.", Livsmedelstyp.Dryck)
            {
                Anvandningsmeddelande = "Kapsylen flyger med ett pys. Du tar en rejäl klunk. Skål!",
            });

            SakerIRummet.Add(new Livsmedel("Lunchbröd", "Ett nybakat bröd, perfekt för en vardagslunch.", Livsmedelstyp.Mat, true, true)
            {
                Anvandningsmeddelande = "Du äter upp det nybakade brödet. Mums!",
            });
        }

        public override string UndersokRum(Spelare spelare, Core.StoryState story)
        {
            string output = "";
            var brod = SakerIRummet.FirstOrDefault(s => s.Namn == "Lunchbröd");

            if (brod != null && brod.ArGomd)
            {
                brod.ArGomd = false; // Nu dyker det upp i rummets lista och JAWS ser det!
                output += "\nDu undersöker det dukade bordet och ser bland annat ett brödfat som är täckt med en handduk.\n";
                output += "Du lyfter på handduken och hittar lunchbröd!\n";
            }
            else
            {
                output += "\nKöket är rent och snyggt. Brödfatet står tomt på bordet.\n";
            }

            // Vi returnerar texten så att fönstret kan visa den
            return output;
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Här kollar vi om spelaren har uteskor (boots)
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                string text = "\n[SYSTEMMEDDELANDE: HYGIENKONTROLL]\n" +
                              "En röst från en dold högtalare vid köksdörren piper:\n" +
                              "'STOPP! Smutsiga uteskor detekterade i en zon för livsmedelshantering.'\n" +
                              "'Vänligen byt till inneskor i hallen innan du beträder Herrskapets kök.'\n\n" +
                              "Du kan inte gå in här med de där skorna.";

                // Visa meddelandet - JAWS fokuserar här direkt
                MessageBox.Show(text, "Hygienkontroll - Tillträde förbjudet", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                // Vi returnerar false eftersom man INTE får kliva in med uteskor
                return false;
            }

            return true;
        }
    }
}
