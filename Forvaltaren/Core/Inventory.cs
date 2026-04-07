using Forvaltaren.Modeller;
using Forvaltaren.Modeller.Saker;

namespace Forvaltaren.Core
{
    public class Inventory
    {
        private List<Spelsak> _saker = new List<Spelsak>();

        public int MarkeratIndex { get; set; } = 0;

        public void LaggTill(Spelsak sak) => _saker.Add(sak);
        public List<Spelsak> GetAllaSaker() => _saker;
        public bool HarForemal(string namn) => _saker.Any(s => s.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase));

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

        public static string HanteraUtrustning(Spelsak sak, Spelare spelare, StoryState story, Inventory inv)
        {
            if (sak is Klader plagg)
            {
                // Kontrollera krockar och lager-ordning (Original-logik)
                if (inv != null && !plagg.ArAktiv)
                {
                    var aktivaPaSammaStalle = inv.GetAllaSaker().OfType<Klader>()
                                                 .Where(k => k.ArAktiv && k.Placering == plagg.Placering);

                    foreach (var annat in aktivaPaSammaStalle)
                    {
                        if (annat.Lager == plagg.Lager)
                        {
                            annat.Anvand(spelare, story);
                        }

                        if (annat.Lager > plagg.Lager)
                        {
                            return $"Du kan inte ta på dig {plagg.Namn.ToLower()} utanpå {annat.Namn.ToLower()}!";
                        }
                    }
                }

                // Kör plaggets logik
                string meddelandeFrånPlagget = plagg.Anvand(spelare, story);

                // ÅTERSTÄLLD LOGIK: Uppdatera AktivtSkodon baserat på dina original-regler
                switch (plagg.Typ)
                {
                    case Bekladnadstyp.Ute:
                    case Bekladnadstyp.Inne:
                    case Bekladnadstyp.Skydd:
                        if (plagg.Placering == Kroppsdel.Fot && plagg.Lager != BekladnadsLager.Underst)
                        {
                            // Om plagget blev aktivt, sätt typen. Annars nollställ.
                            spelare.AktivtSkodon = plagg.ArAktiv ? plagg.Typ.ToString() : "";
                        }
                        break;
                }

                return meddelandeFrånPlagget;
            }

            if (sak is Redskap redskap)
            {
                string svar = redskap.Anvand(spelare, story);
                if (redskap.ArAktiv && inv != null)
                {
                    foreach (var r in inv.GetAllaSaker().OfType<Redskap>())
                    {
                        if (r != redskap) r.ArAktiv = false;
                    }
                }
                return svar;
            }

            if (sak is AllmanSak allmanSak)
            {
                string svar = allmanSak.Anvand(spelare, story);
                if (allmanSak.ArAktiv && inv != null)
                {
                    foreach (var a in inv.GetAllaSaker().OfType<AllmanSak>())
                    {
                        if (a != allmanSak) a.ArAktiv = false;
                    }
                }
                return svar;
            }

            if (sak is Livsmedel livsmedel)
            {
                return livsmedel.Anvand(spelare, story);
            }

            return sak.Anvand(spelare, story);
        }
    }
}