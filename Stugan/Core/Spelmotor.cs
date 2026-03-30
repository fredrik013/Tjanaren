using Stugan.Modeller;
using Stugan.Rooms;

namespace Stugan.Core
{
    public class Spelmotor
    {
        private Spelare _spelare;

        private WorldMap _worldMap;

        private StoryState _story;

        public Spelmotor()
        {
            _story = new StoryState();
            _worldMap = new WorldMap();

            // HÄR ÄR FIXEN: 
            // Vi säger åt motorn att använda metoden som finns i klassen Rumskarta
            Rum startRum = Rumskarta.SkapaVarlden(_worldMap, _story);

            // Nu har vi startRum och kan initiera spelaren
            _spelare = new Spelare(startRum);
        }

        public string HamtaStartText()
        {
            // Vi använder en StringBuilder eller bara vanlig sträng-konkatenering
            string intro = "--- TJÄNARE HOS HERRSKAPET VON KRÅNGEL ---\n\n";
            intro += "Välkommen till Stugan.\n";
            intro += "Regnet piskar mot nacken när du står utanför den gamla träbyggnaden.\n";
            intro += "Du har blivit anställd för att städa upp efter familjen Von Krångel,\n";
            intro += "men de dömande blickarna från fönstren antyder att de förväntar sig underverk.\n\n";

            intro += "Du känner tyngden av ryggsäcken på ryggen,\n";
            intro += "och dina boots är ordentligt snörade på fötterna – bäst att inte smutsa ner finmattorna...\n";
            intro += "\n--------------------------------------------------\n\n";

            // Slutligen lägger vi till rumsbeskrivningen för startrummet (Gårdsplanen)
            intro += _spelare.NuvarandeRum.VisaBeskrivning();

            return intro;
        }

        public string HamtaInventoryTest()
        {
            return "I din ryggsäck hittar du: Ett par boots.";
        }

        public string HanteraInmatning(ConsoleKey tangent)
        {
            // Vi samlar all text i denna sträng för att visa i rtbDisplay
            string output = "";

            switch (tangent)
            {
                case ConsoleKey.I:
                    // Ryggsäckens innehåll + rumsbeskrivning
                    output = _spelare.Ryggsack.Visa(_spelare, _story);
                    output += "\n" + _spelare.NuvarandeRum.VisaBeskrivning();
                    break;

                case ConsoleKey.S:
                    // Status-skärmen
                    output = _spelare.VisaStatus(_story);
                    output += "\n\nTryck på valfri tangent för att återgå till rummet...";
                    // I Form1 rensas texten vid nästa knapptryck, så detta fungerar som din gamla ReadKey
                    break;

                case ConsoleKey.U:
                    output = $"\nDu ser dig noga omkring i {_spelare.NuvarandeRum.Namn}...\n";
                    output += _spelare.NuvarandeRum.UndersokRum(_spelare, _story);
                    break;

                case ConsoleKey.O:
                    // "O" för Objekt - hämtar listan från Rum.cs
                    output = _spelare.NuvarandeRum.VisaObjektLista(_spelare, _story);
                    break;

                case ConsoleKey.T:
                    // Här anropas din metod för att plocka upp saker
                    // (Se till att TaUppSak returnerar en sträng nu!)
                    output = TaUppSak();
                    break;

                case ConsoleKey.B:
                    // Lång beskrivning av rummet
                    output = _spelare.NuvarandeRum.LasLangBeskrivning();
                    break;

                case ConsoleKey.M:
                    // Underhåll (t.ex. starta tvättmaskinen eller laga rör)
                    output = _spelare.NuvarandeRum.UtforUnderhall(_spelare, _story);
                    break;

                case ConsoleKey.UpArrow:
                    output = FlyttaSpelare("Norr");
                    break;

                case ConsoleKey.DownArrow:
                    output = FlyttaSpelare("Soder");
                    break;

                case ConsoleKey.RightArrow:
                    output = FlyttaSpelare("Oster");
                    break;

                case ConsoleKey.LeftArrow:
                    output = FlyttaSpelare("Vaster");
                    break;

                case ConsoleKey.PageUp:
                    output = FlyttaSpelare("Upp");
                    break;

                case ConsoleKey.PageDown:
                    output = FlyttaSpelare("Ner");
                    break;

                default:
                    output = _spelare.NuvarandeRum.VisaBeskrivning();
                    break;
            }

            return output;
        }

        // Vi ändrar den till att returnera en string för att fönstret ska kunna visa den
        private string FlyttaSpelare(string riktning)
        {
            // Vi skapar en lokal variabel för att samla upp texten istället för WriteLine
            string logg = "";
            string exitText;

            bool visadeExit = _spelare.NuvarandeRum.VisaExitMeddelande(riktning, out exitText);

            if (_spelare.NuvarandeRum.Utgangar.ContainsKey(riktning))
            {
                Rum hoppRum = _spelare.NuvarandeRum.Utgangar[riktning];

                if (hoppRum != null)
                {
                    if (hoppRum.KanGaIn(_spelare, _story))
                    {
                        _spelare.NuvarandeRum = hoppRum;
                        // Här behöver vi ändra så VisaBeskrivning returnerar texten
                        logg += _spelare.NuvarandeRum.VisaBeskrivning(visadeExit);
                    }
                    return logg;
                }

                Position? nuvarandePos = _spelare.NuvarandeRum.Plats;
                if (nuvarandePos != null)
                {
                    int x = nuvarandePos.X; int y = nuvarandePos.Y; int z = nuvarandePos.Z;

                    switch (riktning)
                    {
                        case "Norr": y++; break;
                        case "Soder": y--; break;
                        case "Oster": x++; break;
                        case "Vaster": x--; break;
                        case "Upp": z++; break;
                        case "Ner": z--; break;
                    }

                    Position nastaPos = new Position(x, y, z);
                    Rum nastaRum = _worldMap.HamtaRum(nastaPos);

                    if (nastaRum != null)
                    {
                        if (nastaRum.KanGaIn(_spelare, _story))
                        {
                            _spelare.NuvarandeRum = nastaRum;
                            logg += _spelare.NuvarandeRum.VisaBeskrivning(visadeExit);
                        }
                    }
                    else
                    {
                        logg += "Där är det stopp, det finns inget rum åt det hållet.";
                    }
                }
            }
            else
            {
                logg += "Där är det stopp, det finns ingen utgång åt det hållet.";
            }

            return logg;
        }

        // Denna anropas fortfarande av case ConsoleKey.T
        public List<Spelsak> TaUppSak()
        {
            // Vi hämtar de saker som går att ta
            var sakerAttTa = _spelare.NuvarandeRum.SakerIRummet
                .Where(s => s.KanPlockasUpp && !s.ArGomd)
                .ToList();

            // Vi returnerar listan direkt till Form1. 
            // Om listan är tom, vet fönstret att det inte finns något att plocka upp.
            return sakerAttTa;
        }

        // En ny liten hjälpreda som faktiskt flyttar saken när du valt i listan
        public string VerkstallUpplockning(Spelsak valdSak)
        {
            if (valdSak != null)
            {
                _spelare.Ryggsack.LaggTill(valdSak);
                _spelare.NuvarandeRum.SakerIRummet.Remove(valdSak);
                return $"\nDu plockar upp: {valdSak.Namn}.";
            }
            return "\nInget valdes.";
        }
    }
}
