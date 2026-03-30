using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
{
    public class Tvattstuga : Rum
    {
        public Tvattstuga() : base("Tvättstugan", "Du har kommit in i en tvättstuga. Här råder det god ordning och det luktar starkt av tvättmedel.Här finns det både tvättmaskin och torktumlare..")
        {
            VisaNamn = "i tvättstugan";
            OppnaUtgangar("Oster");

            SakerIRummet.Add(new Inredning("Tvättmaskin", "En modern tvättmaskin som ser ut att vara i gott skick. Den är fylld med smutsiga kläder som väntar på att bli rena.", false, false));
            SakerIRummet.Add(new Inredning("Torktumlare", "En torktumlare som matchar tvättmaskinen."));

        }

        public override void UtforUnderhall(Spelare spelare, StoryState story)
        {
            Clear();
            WriteLine("--- UNDERHÅLL I TVÄTTSTUGAN ---");
            WriteLine("1. Starta tvättmaskinen");

            // Nu använder vi den nya smidiga metoden!
            if (spelare.Ryggsack.HarForemal("Boots"))
            {
                WriteLine("2. Putsa dina smutsiga boots");
            }
            WriteLine("3. Avbryt");

            var val = ReadKey(true);
            if (val.Key == ConsoleKey.D1)
            {
                story.TvattmaskinArIgang = true;
                WriteLine("\n[KLICK-SURRRRR]");
                WriteLine("Maskinen låser luckan och börjar snurra. Vattnet väser i rören.");
            }
            else if (val.Key == ConsoleKey.D2 && spelare.Ryggsack.HarForemal("Boots"))
            {
                WriteLine("\nDu tar fram borsten och putsar bort leran från dina boots.");
                WriteLine("De glänser nu som nya.");
                // Här kan vi flagga att de är rena om vi vill senare
            }

            WriteLine("\nTryck på valfri tangent...");
            ReadKey(true);
        }
    }
}
