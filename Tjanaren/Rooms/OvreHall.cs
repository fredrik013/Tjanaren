using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tjanaren.Rooms
{
    public class OvreHall : Rum
    {
        public OvreHall() : base("Övre hallen", "Du står i den övre hallen. Härifrån kan du gå till sovrummen och badrummet.")
        {
        VisaNamn = "i övre hallen";
            OppnaUtgangar("Ner", "Vaster", "Oster", "Norr");
        }
    }
}
