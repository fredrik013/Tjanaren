using Stugan.Core;

namespace Stugan.Modeller
{
    public abstract class Spelsak
    {
        public string Namn { get; set; }

        public string Beskrivning { get; set; }

        public string Anvandningsmeddelande { get; set; } = "";

        public bool KanPlockasUpp { get; set; }

        public bool ArGomd { get; set; }

        public bool ArAktiv { get; set; }

        public bool ForsvinnerVidAnvandning { get; set; } = false;

        public Spelsak(string namn, string beskrivning, bool kanplockasupp = true, bool argomd = false)
        {
            Namn = namn;
            Beskrivning = beskrivning;
            KanPlockasUpp = kanplockasupp;
            ArGomd = argomd;
            ArAktiv = false;
        }

        public abstract string Anvand(Spelare spelare, StoryState story);
    }
}
