using Tjanaren.Core;
using Tjanaren.Modeller;

namespace Tjanaren.Modeller.Saker
{
    public class Redskap : Spelsak
    {
        public bool ArRen { get; set; } = true;

        public Redskap(string namn, string beskrivning, bool kanPlockasUpp = true, bool arGomd = false, bool arAktiv = false, bool arRen = true)
       : base(namn, beskrivning)
        {
            KanPlockasUpp = kanPlockasUpp;
            ArGomd = arGomd;
            ArAktiv = arAktiv;
            ArRen = arRen;
        }

        public override string Anvand(Spelare spelare, StoryState story)
        {
            // Vi skiftar status: var den aktiv blir den inaktiv och tvärtom
            ArAktiv = !ArAktiv;

            if (ArAktiv)
            {
                return $"Du tar fram {Namn} och håller den i ett stadigt grepp.";
            }
            else
            {
                return $"Du stoppar ner {Namn} i ryggsäcken igen.";
            }
        }
    }
}
