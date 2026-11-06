using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Bibliotek : Rum
    {
        public Bibliotek() : base("Biblioteket", "Du står i biblioteket. Här kan du läsa böcker och studera.")
        {
            VisaNamn = "i biblioteket";
            OppnaUtgangar("Vaster");
        }
    }
}
