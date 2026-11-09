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

                case Sovrum sovrum:
                    switch (s.Ryggsack.GetAllaSaker()
                        .OfType<Klader>()
                        .FirstOrDefault(k => k.Namn == "Sidenkalsonger"))
                    {
                        case Klader k when !k.ArAktiv:
                            meddelande += "\nDu går ut från sovrummet och springer rakt i armarna på herr von Krångel. Han blir misstänksam och börjar rota i din ryggsäck. Plötsligt får han tag i de exklusiva sidenkalsongerna och utbrister: ”Vad i hela friden! Du understår dig väl inte att stjäla mina exklusiva sidenkalsonger! Det här blir en prick i tjänsteprotokollet!” Herr von Krångel tar sidenkalsongerna ur ryggsäcken och går med arga steg in i sovrummet med dem, muttrande för sig själv: ”Det är märkligt att inte tjänstefolket kan lämna mina saker i fred!”";
                            s.Ryggsack.GetAllaSaker().Remove(k);
                            sovrum.SakerIRummet.Add(k);
                            return true;


                        default:
                            meddelande += "\nDu går ut från sovrummet och springer rakt i armarna på herr von Krångel. Han blir misstänksam och börjar rota i din ryggsäck men hittar ingenting misstänkt där. Du hör honom muttra: ”Jävla tjänstefolk som snokar i mitt sovrum.”";
                            return true;
                    }


                case VonKrangelBadrum _:
                    meddelande += "Du smyger tyst in i den övre hallen från badrummet.\n";
                    return true;
                default:
                    return true;
            }
        }
    }
}
