using Tjanaren.Core;
using Tjanaren.Modeller;

namespace Tjanaren.Modeller.Saker
{
    public class AllmanSak : Spelsak
    {
        public Action<Spelare>? Specialeffekt { get; set; }

        public AllmanSak(string namn, string beskrivning, bool kanplockasupp = true, bool argomd = false, bool arAktiv = false)
            : base(namn, beskrivning, kanplockasupp, argomd)
        {
            ArAktiv = arAktiv;
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
