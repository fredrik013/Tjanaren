using Forvaltaren.Core;
using Forvaltaren.Modeller;

namespace Forvaltaren.Modeller.Saker
{
    public class Inredning : Spelsak
    {
        public Inredning(string namn, string beskrivning, bool kanplockasupp = false, bool argomd = false)
            : base(namn, beskrivning, kanplockasupp, argomd)
        {

        }

        public override string Anvand(Spelare spelare, StoryState story)
        {
            return "Du kan inte använda " + Namn + ".";
        }
    }
}
