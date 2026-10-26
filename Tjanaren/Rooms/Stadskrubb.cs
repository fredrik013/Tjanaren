using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Core;
using Tjanaren.Modeller.Saker;
namespace Tjanaren.Rooms
{
    public class Stadskrubb : Rum
    {
        public Stadskrubb() : base("Städskrubb", "Du står i en liten städskrubb. Det är trångt här inne, men du kan skymta några gamla föremål som ligger huller om buller på golvet. " +
            "Du lägger märke till en bänk med en ho där man kan fylla på vatten för att rengöra skurmopp med mera. Under bänken finns ett skåp. Det finns en dörr som leder till tvättstugan.")
        {
            VisaNamn = "i städskrubben";
            OppnaUtgangar("Oster");
            SakerIRummet.Add(new Redskap(
                            "Hink",
                            "En hink som antagligen ska användas vid städning.",
arAktiv: true
                        ));
            SakerIRummet.Add(new Redskap(
                            "Mopp",
                            "En gammal mopp som ser ut att ha sett bättre dagar.",
                            arAktiv: true
                        ));
            SakerIRummet.Add(new Redskap(
                            "Sopborste",
                            "En sopborste som ser ut att ha använts flitigt.",
                            arAktiv: true
                        ));
            SakerIRummet.Add(new Redskap(
                            "Dammsugare",
                            "En dammsugare som kan användas för att dammsuga de fina mattorna med.",
                            arAktiv: true
                        ));
            SakerIRummet.Add(new AllmanSak(
                            "Skurmedel",
                            "En flaska skurmedel som ska blandas ut med vatten.",
                            argomd: true,
                            arAktiv: true
                        ));
        }
        public override bool HarUnderhallsmeny => true;
        private bool _hinkHarVatten = false;


        public override string UndersokRum(Spelare spelare, StoryState story)
        {
            string output = "";
            var skurmedel = SakerIRummet.Find(s => s.Namn.ToLower().Contains("skurmedel"));

            if (!HarUndersokts)
            {
                // Logiken är intakt: Vi gör skurmedlet synligt
                if (skurmedel != null) skurmedel.ArGomd = false;

                output += "\nDu rotar i skåpet under bänken...\n";
                output += "Där, längst in bland en massa skräp, hittar du en flaska med skurmedel!\n";
                HarUndersokts = true;
            }
            else
            {
                output += "\nDu ser inget mer av värde i skåpet, bara meningslöst skräp.\n";
            }

            // HÄR ÄR RÄDDNINGEN: Vi returnerar hela strängen till spelmotorn
            return output;
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            string meny = "--- UNDERHÅLL I STÄDSKRUBBEN ---\n";
            meny += "1. Fyll skurhinken med vatten\n";

            meny += "2. Blanda skurmedel med vatten\n";

            meny += "3. Skölj rent hinken\n";

            meny += "Escape. Avbryt\n";
            return meny;
        }

        public override string HanteraUnderhallsval(
ConsoleKey val,
Spelare spelare,
StoryState story)
        {
            if (val == ConsoleKey.D1)
            {
                // Kontrollera att spelaren håller hinken i handen.
                if (!spelare.Ryggsack.GetAllaSaker()
                    .Any(s => s.Namn.Equals("Hink", StringComparison.OrdinalIgnoreCase)
                              && s.ArAktiv))
                {
                    return "\nDu måste hålla hinken i handen för att kunna fylla den med vatten.";
                }

                // Kontrollera om skurvattnet redan är färdigblandat.
                if (story.SkurvattenKlart)
                {
                    return "\nHinken innehåller redan färdigblandat skurvatten.";
                }

                // Kontrollera om hinken redan innehåller vatten.
                if (_hinkHarVatten)
                {
                    return "\nHinken är redan fylld med vatten.";
                }

                // Fyll hinken.
                _hinkHarVatten = true;

                return "\nDu ställer hinken under kranen och fyller den med vatten.";
            }

            else if (val == ConsoleKey.D2)
            {
                // Kontrollera att skurvattnet inte redan är färdigblandat.
                if (story.SkurvattenKlart)
                {
                    return "\nSkurvattnet är redan färdigblandat.";
                }

                // Kontrollera att hinken innehåller vatten.
                if (!_hinkHarVatten)
                {
                    return "\nDu måste fylla hinken med vatten först.";
                }

                // Kontrollera att spelaren håller skurmedlet.
                if (!spelare.Ryggsack.GetAllaSaker()
                    .Any(s => s.Namn.Equals("Skurmedel", StringComparison.OrdinalIgnoreCase)
                              && s.ArAktiv))
                {
                    return "\nDu måste ha skurmedlet i handen för att kunna blanda skurvattnet.";
                }

                // Blanda skurvattnet.
                story.SkurvattenKlart = true;

                return "\nDu häller skurmedlet i hinken och rör om. Skurvattnet är nu färdigt att användas.";
            }

            else if (val == ConsoleKey.Escape)
            {
                return "\nDu avbryter underhållet.";
            }

            return "";
        }
    }
}
