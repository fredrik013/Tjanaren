using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Sovrum : Rum
    {
        public Sovrum() : base("Sovrummet", "Du står i sovrummet. Här kan du vila och återhämta dig.")
        {
            VisaNamn = "i sovrummet";
            OppnaUtgangar("Oster");
        }
    }
}
