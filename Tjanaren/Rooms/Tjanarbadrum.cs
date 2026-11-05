using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Tjanarbadrum : Rum
    {
        public Tjanarbadrum() : base("Tjänarens badrum", "Du kliver in i ett litet och bedrövligt badrum. " +
"Luften är fuktig och luktar unket. Kaklet har en gång " +
"varit vitt, men har numera antagit en obestämd gulgrå nyans. " +
"En ensam glödlampa sprider ett sorgligt ljus över eländet. " +
"I ena hörnet finns en dusch som ser ut att ha överlevt " +
"åtminstone tre generationer tjänstefolk.")
        {
            VisaNamn = "i tjänarens badrum";
            OppnaUtgangar("Norr");

            SakerIRummet.Add(new Inredning("Dusch",
                "Du granskar duschen närmare. Duschmunstycket är täckt av " +
"kalkavlagringar och slangen har sett betydligt bättre dagar. " +
"Mellan kakelplattorna frodas svartmöglet, och på golvet ligger " +
"en gammal tvål som någon tycks ha glömt för flera år sedan. " +
"En nästan tom schampoflaska står på en rostig hylla. " +
"På blandaren sitter en handskriven lapp: " +
"\"VARMVATTEN KOSTAR PENGAR! /H. von Krångel\"",
                false, false));
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            var aktivaKlader = spelare.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .Where(k => k.ArAktiv)
                .ToList();

            if (aktivaKlader.Any())
            {
                return "\nDu tänker kliva in i duschen, men inser " +
                       "att du fortfarande har kläder på dig. " +
                       "Det är nog bäst att ta av sig allt först.\n";
            }

            story.ArNyduschad = true;

            return "\nDu kliver försiktigt in i den gamla duschen " +
                   "och vrider på kranen. Rören protesterar med " +
                   "ett högljutt skramlande innan en tunn stråle " +
                   "ljummet vatten börjar rinna.\n\n" +
                   "Du plockar upp den gamla tvålen och tvålar " +
                   "omsorgsfullt in kroppen. Därefter lyckas du " +
                   "pressa ut de sista dropparna ur schampoflaskan " +
                   "och schamponerar håret.\n\n" +
                   "Efter en ordentlig avsköljning stänger du " +
                   "kranen och kliver ut. Trots duschens bedrövliga " +
                   "skick känner du dig äntligen ren och fräsch.\n\n" +
                   "Herr von Krångel skulle förmodligen kalla " +
                   "detta ett oacceptabelt slöseri med varmvatten.\n";
        }
    }
}
