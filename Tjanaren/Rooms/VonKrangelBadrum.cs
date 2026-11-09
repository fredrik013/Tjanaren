using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class VonKrangelBadrum : Rum
    {
        public VonKrangelBadrum() : base("Von Krångels Badrum", "Du har kommit in i von Krångels exklusiva badrum. " + "Här ser man att han gör skillnad på folk och folk. Inredningen är exklusiv med ett rejält tvättställ. men det stora bubbelbadkaret fångar din uppmärksamhet.")
        {
            VisaNamn = "i Von Krångel's badrum";
            OppnaUtgangar("Soder");
            SakerIRummet.Add(new AllmanSak("Rakhyvel", "Det här ser verkligen ut att vara en exklusiv rakhyvel", true, false, true));

            SakerIRummet.Add(new Inredning ("Bubbelbadkar", "Det här ser verkligen ut att vara ett exklusivt badkar med värme och bubblor.", false, false));
        }
    }
}
