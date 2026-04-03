using Stugan.Modeller;
using Stugan.Modeller.Saker;

namespace Stugan.Core
{
    public class Inventory
    {
        private List<Spelsak> _saker = new List<Spelsak>();

        // Den här håller reda på vad användaren valt i ListBoxen
        public int MarkeratIndex { get; set; } = 0;

        public void LaggTill(Spelsak sak) => _saker.Add(sak);
        public List<Spelsak> GetAllaSaker() => _saker;
        public bool HarForemal(string namn) => _saker.Any(s => s.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase));

        // Ny metod som ersätter den gamla loopen
        public string HanteraInmatning(ConsoleKey tangent, Spelare spelare, StoryState story, Action stangMeny)
        {
            if (_saker.Count == 0) return "Ryggsäcken är tom.";

            switch (tangent)
            {
                case ConsoleKey.Enter:
                    var valdSak = _saker[MarkeratIndex];
                    string svar = HanteraUtrustning(valdSak, spelare, story, this);

                    if (valdSak.ForsvinnerVidAnvandning)
                        _saker.Remove(valdSak);

                    return svar;

                case ConsoleKey.U:
                    var sakAttSe = _saker[MarkeratIndex];
                    return string.IsNullOrWhiteSpace(sakAttSe.Beskrivning)
                           ? $"Det finns inget särskilt att notera om {sakAttSe.Namn}."
                           : sakAttSe.Beskrivning;

                case ConsoleKey.Delete:
                    var sakAttSlappa = _saker[MarkeratIndex];

                    if (sakAttSlappa is Klader plagg)
                    {
                        plagg.ArAktiv = false;
                        if (plagg.Typ == Bekladnadstyp.Ute || plagg.Typ == Bekladnadstyp.Inne || plagg.Typ == Bekladnadstyp.Skydd)
                            spelare.AktivtSkodon = "";
                    }

                    spelare.NuvarandeRum.SakerIRummet.Add(sakAttSlappa);
                    _saker.RemoveAt(MarkeratIndex);

                    return $"Du lämnade {sakAttSlappa.Namn} i {spelare.NuvarandeRum.Namn}.";

                case ConsoleKey.Escape:
                case ConsoleKey.I:
                    stangMeny();
                    return "";
            }

            return "";
        }

        // Din befintliga HanteraUtrustning (behåll den exakt som den är, den är kanon!)
        public static string HanteraUtrustning(Spelsak sak, Spelare spelare, StoryState story, Inventory inv)
        {
            // ... (Kopiera in din logik för kläder, redskap etc. här) ...
            if (sak is Klader plagg)
            {
                // Din lager-logik här...
                return plagg.Anvand(spelare, story);
            }
            // ... resten av din kod ...
            return sak.Anvand(spelare, story);
        }
    }
}