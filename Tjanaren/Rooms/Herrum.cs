using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Herrum : Rum
    {
        public Herrum() : base("Herrum", "Du kliver in i ett litet och kallt rum. " +
"Väggarna är täckta av en gammal, skrovald tapet. " +
"En gammal säng ligger i mitten av rummet, ovanför " +
"vilken en liten, röd kudde ligger. " +
"En gammal bokhylla står i hörnet, och på den ligger " +
"en rad böcker med slöjiga omslag.")
        {
            VisaNamn = "i herrummet";
            OppnaUtgangar("Vaster");

        }
    }
}
