using Stugan.Core;

namespace Stugan.Modeller.Saker
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
