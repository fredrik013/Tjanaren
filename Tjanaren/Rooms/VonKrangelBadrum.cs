using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class VonKrangelBadrum : Rum
    {
        public VonKrangelBadrum() : base("Von Krångels Badrum", "Du har kommit in i von Krångels exklusiva badrum. " + "Här ser man att han gör skillnad på folk och folk. Inredningen är exklusiv med ett rejält tvättställ. Men det stora bubbelbadkaret fångar din uppmärksamhet.")
        {
            VisaNamn = "i Von Krångels badrum";
            OppnaUtgangar("Soder");
            SakerIRummet.Add(new AllmanSak("Rakhyvel", "Det här ser verkligen ut att vara en exklusiv rakhyvel", true, false, true));

            SakerIRummet.Add(new Inredning ("Bubbelbadkar", "Det här ser verkligen ut att vara ett exklusivt badkar med värme och bubblor. Du känner dig frestad att prova det men vågar du ta risken med tanke på husets regler?", false, false));
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            story.HarAnvantSpabad = true;

            return "\nDu bestämmer dig för att unna dig ett riktigt " +
       "lyxigt bad. Du börjar med att tappa upp vatten i " +
       "det stora bubbelbadkaret. När badkaret är fyllt " +
       "klär du av dig och kliver försiktigt ner i det " +
       "varma vattnet.\n\n" +
       "Du lutar dig tillbaka och njuter av bubblorna. " +
       "Det här är sannerligen något helt annat än den " +
       "gamla duschen i källaren.\n\n" +
       "Plötsligt rycks dörren upp.\n\n" +
       "”VAD I HELVETE ÄR DET SOM FÖRSIGGÅR HÄR?!”\n\n" +
       "Herr von Krångel står i dörröppningen, illröd i ansiktet.\n\n" +
       "”LIGGER DU OCH BADAR I MITT PRIVATA SPABAD?! " +
       "HAR DU FULLSTÄNDIGT FÖRLORAT FÖRSTÅNDET?”\n\n" +
       "”TJÄNSTEFOLK HAR ETT EGET BADRUM! DÄR FINNS EN " +
       "FULLT FUNGERANDE DUSCH! ANVÄND DEN!”\n\n" +
       "”OCH FÖRSVINN UR MITT BADKAR INNAN JAG LÅTER " +
       "TÖMMA DET MED DIG KVAR I!”\n\n" +
       "Du kliver motvilligt ur det varma spabadet medan " +
       "Herr von Krångel fortsätter att skälla ut dig. " +
       "Du torkar dig snabbt innan du riskerar att följa med ner i avloppet.\n";
        }
    }
}
