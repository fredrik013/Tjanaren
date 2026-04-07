using Forvaltaren.Core;
using Forvaltaren.Modeller;

namespace Forvaltaren.Modeller.Saker
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
                    story.AntalOvertramp++;
                    break;

                default:
                    break;

            }

            return Anvandningsmeddelande;
        }

    }
}
