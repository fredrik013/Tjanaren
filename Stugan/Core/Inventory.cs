using Stugan.Modeller;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Core
{
    public class Inventory
    {
        private List<Spelsak> _saker = new List<Spelsak>();
        private int _markeratIndex = 0;

        public void LaggTill(Spelsak sak) => _saker.Add(sak);
        public List<Spelsak> GetAllaSaker() => _saker;

        // Lägg till denna i Inventory.cs
        public bool HarForemal(string namn) => _saker.Any(s => s.Namn.Equals(namn, StringComparison.OrdinalIgnoreCase));

        public void AvaktiveraTyp(Bekladnadstyp typ)
        {
            foreach (var sak in _saker)
            {
                if (sak is Klader klader && klader.Typ == typ)
                    klader.ArAktiv = false;
            }
        }

        public void Visa(Spelare spelare, StoryState story)
        {
            if (_saker.Count == 0)
            {
                WriteLine("Ryggsäcken är tom."); // Standard WriteLine
                return;
            }

            bool tittar = true;
            _markeratIndex = 0;

            Clear();
            WriteLine("RYGGSÄCK");
            for (int i = 0; i < _saker.Count; i++)
            {
                var sak = _saker[i];
                string status = "";

                // Hämta statusen direkt här
                if (sak is Klader p) status = p.ArAktiv ? " (påtagen)" : " (i ryggsäcken)";
                else if (sak is Redskap r) status = r.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is AllmanSak a) status = a.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is Livsmedel l) status = l.ArAktiv ? " (i handen)" : " (i ryggsäcken)";


                // Skriv ut hela raden på en gång, inkl status
                WriteLine($"   {sak.Namn}{status}");
            }

            UppmarksammaRad();

            while (tittar)
            {
                var k = ReadKey(true).Key;

                switch (k)
                {
                    // Inne i switch (k) i Inventory.cs:

                    case ConsoleKey.U:
                        var sakAttSe = _saker[_markeratIndex];

                        // Vi hämtar den befintliga beskrivningen från objektet
                        string info = string.IsNullOrWhiteSpace(sakAttSe.Beskrivning)
                                      ? $"Det finns inget särskilt att notera om {sakAttSe.Namn}."
                                      : sakAttSe.Beskrivning;

                        // Skriv ut på rad 12 så JAWS läser upp det direkt
                        SetCursorPosition(0, 12);
                        WriteLine(info.PadRight(WindowWidth));

                        // Vi pausar lite så man hinner höra beskrivningen innan man trycker vidare
                        Thread.Sleep(500);
                        break;

                    case ConsoleKey.DownArrow:
                        if (_markeratIndex < _saker.Count - 1)
                        {
                            _markeratIndex++;
                            UppmarksammaRad();
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        if (_markeratIndex > 0)
                        {
                            _markeratIndex--;
                            UppmarksammaRad();
                        }
                        break;

                    case ConsoleKey.Enter:
                        var valdSak = _saker[_markeratIndex];
                        string svar = HanteraUtrustning(valdSak, spelare, story, this);

                        if (_saker.Contains(valdSak))
                        {
                            // 1. Uppdatera statusen på raden där vi står (Skriv direkt)
                            SetCursorPosition(0, _markeratIndex + 1);
                            string status = "";
                            if (valdSak is Klader p) status = p.ArAktiv ? " (påtagen)" : " (i ryggsäcken)";
                            else if (valdSak is Redskap r) status = r.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                            else if (valdSak is AllmanSak a) status = a.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                            else if (valdSak is Livsmedel l) status = l.ArAktiv ? " (i handen)" : " (i ryggsäcken)";

                            Write($"   {valdSak.Namn}{status}".PadRight(45));

                            // 2. Skriv svaret på rad 12
                            UppmarksammaRad();
                            SetCursorPosition(0, 12);
                            Write(svar);
                            if (valdSak.ForsvinnerVidAnvandning) _saker.Remove(valdSak);
                        }
                        else
                        {
                            // Om saken försvann (äten/drickbar)
                            if (_saker.Count == 0) tittar = false;
                            else Visa(spelare, story);

                            SetCursorPosition(0, 12);
                            Write(svar.PadRight(60));
                            return;
                        }
                        break;

                    case ConsoleKey.Escape:
                    case ConsoleKey.I:
                        tittar = false;
                        break;

                    case ConsoleKey.Delete:
                        var sakAttSlappa = _saker[_markeratIndex];

                        // Om det är kläder vi har på oss, ta av dem först så statusen blir rätt
                        if (sakAttSlappa is Klader plagg)
                        {
                            plagg.ArAktiv = false;
                            if (plagg.Typ == Bekladnadstyp.Ute || plagg.Typ == Bekladnadstyp.Inne || plagg.Typ == Bekladnadstyp.Skydd)
                                spelare.AktivtSkodon = "";
                        }

                        // Flytta från ryggsäck till rummet
                        spelare.NuvarandeRum.SakerIRummet.Add(sakAttSlappa);
                        _saker.RemoveAt(_markeratIndex);

                        // Bekräftelse till användaren
                        SetCursorPosition(0, 12);
                        WriteLine($"Du lämnade {sakAttSlappa.Namn} i {spelare.NuvarandeRum.Namn}.".PadRight(WindowWidth));

                        Thread.Sleep(1000); // Paus för JAWS

                        if (_saker.Count == 0)
                        {
                            tittar = false;
                        }
                        else
                        {
                            // Justera index så vi inte hamnar utanför listan
                            if (_markeratIndex >= _saker.Count) _markeratIndex = _saker.Count - 1;

                            // Rita om och fortsätt
                            Visa(spelare, story);
                            return;
                        }
                        break;
                }
            }
        }

        // Hjälpmetod för att rita hela skärmen utan att krascha logiken
        private void RitaHelaMenyn()
        {
            Clear();
            WriteLine("RYGGSÄCK");
            for (int i = 0; i < _saker.Count; i++)
            {
                string markor = i == _markeratIndex ? "> " : "  ";
                string status = "";
                if (_saker[i] is Klader p) status = p.ArAktiv ? " (påtagen)" : " (i ryggsäcken)";
                WriteLine($"{markor}{_saker[i].Namn}{status}");
            }
        }

        public static string HanteraUtrustning(Spelsak sak, Spelare spelare, StoryState story, Inventory inv)
        {
            // 1. Hantera Kläder (Lager, Placering och Skostatus)
            if (sak is Klader plagg)
            {
                // Kontrollera krockar och lager-ordning INNAN vi tar på oss plagget
                if (inv != null && !plagg.ArAktiv)
                {
                    var aktivaPaSammaStalle = inv.GetAllaSaker().OfType<Klader>()
                                                 .Where(k => k.ArAktiv && k.Placering == plagg.Placering);

                    foreach (var annat in aktivaPaSammaStalle)
                    {
                        // REGEL A: Samma lager? Ta av det gamla plagget först.
                        if (annat.Lager == plagg.Lager)
                        {
                            annat.Anvand(spelare, story);
                        }

                        // REGEL B: Försöker vi sätta på något UNDER ett befintligt lager?
                        // (T.ex. Strumpor när Skor redan är på)
                        if (annat.Lager > plagg.Lager)
                        {
                            return $"Du kan inte ta på dig {plagg.Namn.ToLower()} utanpå {annat.Namn.ToLower()}!";
                        }
                    }
                }

                // Kör plaggets egen logik (växlar ArAktiv och pratar med rummet)
                string meddelandeFrånPlagget = plagg.Anvand(spelare, story);

                // Uppdatera spelarens AktivtSkodon baserat på dina original-switchar
                switch (plagg.Typ)
                {
                    case Bekladnadstyp.Ute:
                    case Bekladnadstyp.Inne:
                    case Bekladnadstyp.Skydd:
                        // Vi uppdaterar bara skostatus om det är fötternas yttersta/mellersta lager
                        if (plagg.Placering == Kroppsdel.Fot && plagg.Lager != BekladnadsLager.Underst)
                        {
                            spelare.AktivtSkodon = plagg.ArAktiv ? plagg.Typ.ToString() : "";
                        }
                        break;
                }

                return meddelandeFrånPlagget;
            }

            // 2. Hantera Redskap (Behåll din existerande logik)
            if (sak is Redskap redskap)
            {
                string svar = redskap.Anvand(spelare, story);

                if (redskap.ArAktiv && inv != null)
                {
                    foreach (var r in inv.GetAllaSaker().OfType<Redskap>())
                    {
                        if (r != redskap)
                        {
                            r.ArAktiv = false;
                        }
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
                        if (a != allmanSak)
                        {
                            a.ArAktiv = false;
                        }
                    }
                }
                return svar;
            }

            if (sak is Livsmedel livsmedel)
            {
                return livsmedel.Anvand(spelare, story);
            }

            // 3. Fallback för vanliga saker
            return sak.Anvand(spelare, story);
        }

        private void UppmarksammaRad()
        {
            if (_saker.Count == 0) return;
            // Flytta bara markören till den aktuella raden i listan
            SetCursorPosition(3, _markeratIndex + 1);
        }
    }
}
