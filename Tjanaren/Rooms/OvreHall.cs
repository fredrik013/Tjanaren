using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class OvreHall : Rum
    {
        public OvreHall() : base("Övre hallen", "Du har kommit upp i den övre hallen. Här har det uppenbarligen inte snålats på kostnaderna. Golvet täcks av en tjock, exklusiv matta och på väggarna hänger dyrbara tavlor. Här finns tre dörrar som leder till husets övriga rum.")
        {
            VisaNamn = "i övre hallen";
            OppnaUtgangar("Ner", "Vaster", "Oster", "Norr");
        }

        public override bool KanGaIn(Spelare s, StoryState story, out string meddelande)
        {
            meddelande = "";

            switch (s.NuvarandeRum)
            {
                case InreHall _:
                    switch (s.AktivtSkodon)
                    {
                        case var skodon when skodon == Bekladnadstyp.Ute.ToString():
                            meddelande += "Du klampar upp för trappan. Du hinner precis upp för den då en röst från en högtalare vrålar: ”VAD HAR DU HÄR UPPE ATT GÖRA?! Det här är ingen plats för tjänstefolket!”\n";
                            return false;

                        case var skodon when !s.Ryggsack.GetAllaSaker()
    .OfType<Klader>()
    .Any(k => k.ArAktiv && k.Placering == Kroppsdel.Fot):
                            meddelande += "Du tassar upp för trappan och hinner precis sätta fötterna på övervåningen när du hör ett vrål: ”Men herregud! Tänker du verkligen gå omkring barfota på min exklusiva matta? Dina svettiga fötter lämnar ju våta fläckar! Hela hallen stinker som en ostfabrik!”";
                            return false;

                        default:
                            meddelande += "Du smyger tyst upp för trappan.\n";
                            return true;
                    }                    

                case Sovrum _:
                    meddelande += "Du smyger tyst in i den övre hallen.\n";
                    return true;

                case VonKrangelBadrum _:
                    meddelande += "Du smyger tyst in i den övre hallen från badrummet.\n";
                    return true;
                default:
                    return true;
            }
        }
    }
}
