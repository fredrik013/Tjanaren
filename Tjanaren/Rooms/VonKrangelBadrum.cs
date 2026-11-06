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
        public VonKrangelBadrum() : base("Von Krångel's Badrum", "Du står i Von Krångel's badrum. Här kan du tvätta dig och ändra kläder.")
        {
            VisaNamn = "i Von Krångel's badrum";
            OppnaUtgangar("Soder");
        }
    }
}
