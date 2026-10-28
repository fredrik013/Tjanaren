using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Tvattstuga : Rum
    {
        private readonly WorldMap _karta;

        public Tvattstuga(WorldMap karta) : base("Tvättstugan", "Du har kommit in i en tvättstuga. Här råder det god ordning och det luktar starkt av tvättmedel.Här finns det både tvättmaskin och torktumlare.")
        {
            _karta = karta;
            VisaNamn = "i tvättstugan";
            OppnaUtgangar("Oster", "Vaster");

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
            var mopp = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>()
                .FirstOrDefault(r => r.Namn == "Mopp");

            if (val == ConsoleKey.D1)
            {
                if (story.TvattmaskinArIgang)
                {
                    return "Tvättmaskinen är redan igång.";
                }

                if (mopp == null)
                    return "Du har inget som behöver tvättas.";

                if (mopp.ArRen)
                    return "Det finns ingen anledning att tvätta något som redan är rent.";

                spelare.Ryggsack.GetAllaSaker().Remove(mopp);
                story.TvattmaskinArIgang = true;
                _ = AvslutaTvattAsync(mopp, story);

                return "\nDu slänger in den skitiga moppen i tvättmaskinen, väljer program och trycker på start. [KLICK-SURRRRR]\n" +
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

        private async Task AvslutaTvattAsync(Redskap mopp, StoryState story)
        {
            await Task.Delay(TimeSpan.FromMinutes(2));

            mopp.ArRen = true;

            if (_karta.HamtaRum(new Position(-2, 1, -1))
                is Stadskrubb stadskrubb)
            {
                stadskrubb.SakerIRummet.Add(mopp);
            }

            story.TvattmaskinArIgang = false;
        }
    }
}
