using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
