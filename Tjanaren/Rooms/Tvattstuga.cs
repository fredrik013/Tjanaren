using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
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

        public override bool HarUnderhallsmeny => true;

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            string meny = "--- UNDERHÅLL I TVÄTTSTUGAN ---\n";
            meny += "1. Starta tvättmaskinen\n";

                meny += "2. Putsa dina smutsiga boots\n";
            
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
                if (story.TvattmaskinArIgang)
                {
                    return "Tvättmaskinen är redan igång.";
                }

                story.TvattmaskinArIgang = true;

                return "\n[KLICK-SURRRRR]\n" +
                       "Maskinen låser luckan och börjar snurra. Vattnet väser i rören.";
            }

            else if (val == ConsoleKey.D2)
            {
                if (spelare.Ryggsack.GetAllaSaker()
                    .Any(s => s.Namn.Equals("Boots", StringComparison.OrdinalIgnoreCase)
                              && s.ArAktiv))
                {
                    return "Är du så vig att du kan putsa dina boots medan du fortfarande har dem på fötterna?";
                }

                return "Du hittar en skoborste i en låda och putsar bort leran från bootsen.\n" +
                       "De glänser nu som nya.";
            }

            else if (val == ConsoleKey.Escape)
            {
                return "\nDu avbryter underhållet.";
            }

            return "";
        }
    }
}
