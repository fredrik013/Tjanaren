using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

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

        public override void UndersokRum(Spelare spelare, Core.StoryState story)
        {
            var rum = spelare.NuvarandeRum;
            var brod = rum.SakerIRummet.FirstOrDefault(s => s.Namn == "Lunchbröd");

            if (brod != null && brod.ArGomd)
            {
                brod.ArGomd = false; // Nu dyker det upp i rummets lista!
                WriteLine("\nDu undersöker det dukade bordet och ser bland annat ett brödfat som är täckt med en handduk.");
                WriteLine("Du lyfter på handduken och hittar lunchbröd!");
            }
            else
            {
                WriteLine("\nKöket är rent och snyggt. Brödfatet står tomt på bordet.");
            }
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi kollar om spelaren har på sig något av typen "Ute"
            // Vi använder ToLower() för att vara säkra, ifall vi råkat skriva "ute" med litet u på något plagg.
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                WriteLine("Stopp! Du kan inte gå in i köket med uteskor, du smutsar ner det fina golvet!");
                return false;
            }

            // Om AktivtSkodon är "Inne" eller en tom sträng (barfota) släpps man förbi.
            return true;
        }
    }
}