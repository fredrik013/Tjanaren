using Tjanaren.Core;
using Tjanaren.Modeller;

namespace Tjanaren.Modeller.Saker
{
    public enum Livsmedelstyp
    {
        Mat = 0,
        Dryck = 1,
        Sprit = 2
    }

    public class Livsmedel : Spelsak
    {
        public Livsmedelstyp Typ { get; set; }

        public Livsmedel(string namn, string beskrivning, Livsmedelstyp Typ, bool kanPlockasUpp = true, bool arGomd = false)
            : base(namn, beskrivning)
        {
            this.Typ = Typ;
            KanPlockasUpp = kanPlockasUpp;
            ArGomd = arGomd;
        }

        public override string Anvand(Spelare spelare, StoryState story)
        {
            // Allt livsmedel försvinner när det används
            ForsvinnerVidAnvandning = true;

            // Logik baserad på typ
            switch (Typ)
            {
                case Livsmedelstyp.Mat:
                    story.HarAtitBrod = true;
                    break;

                case Livsmedelstyp.Sprit:
                    story.HarDruckitSprit = true;
                    story.AntalOvertramp++;
                    return "Du öppnar flaskan och tar en klunk. " +
                           "Du hinner knappt svälja innan herr von Krångel dyker upp. " +
                           "\"Vad i hela helvete tar du dig till?! " +
                           "Du understår dig väl inte till att dricka upp min finaste whisky!\"";

                default:
                    break;

            }

            return Anvandningsmeddelande;
        }

    }
}
