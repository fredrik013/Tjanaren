using Tjanaren.Modeller;
using Tjanaren.Rooms;

namespace Tjanaren.Core
{
    public class Spelmotor
    {
        public Spelare _spelare;

        private WorldMap _worldMap;

        private StoryState _story;

        private bool _isInventoryOpen = false;
        public bool IsInventoryOpen => _isInventoryOpen;
        public Inventory Ryggsack => _spelare.Ryggsack;

        private bool _isRoomMenuOpen = false;
        public bool IsRoomMenuOpen => _isRoomMenuOpen;

        // Nytt: Hantering av föremålsval
        private bool _isSelectingItem = false;
        public bool IsSelectingItem => _isSelectingItem;

        private List<Spelsak> _itemsToSelect = new List<Spelsak>();
        public List<Spelsak> ItemsToSelect => _itemsToSelect;

        public int SelectedItemIndex { get; set; } = 0;

        public Spelmotor()
        {
            _story = new StoryState();
            _worldMap = new WorldMap();

            Rum startRum = Rumskarta.SkapaVarlden(_worldMap, _story);
            _spelare = new Spelare(startRum);
        }

        public string HamtaStartText()
        {
            string intro = "--- TJÄNARE HOS HERRSKAPET VON KRÅNGEL ---\n\n";
            intro += "Välkommen till Tjänaren!\n";
            intro += "Regnet piskar mot nacken när du står utanför den gamla träbyggnaden.\n";
            intro += "Du har blivit anställd för att städa upp efter familjen Von Krångel,\n";
            intro += "men de dömande blickarna från fönstren antyder att de förväntar sig underverk.\n\n";

            intro += "Du känner tyngden av ryggsäcken på ryggen,\n";
            intro += "och dina boots är ordentligt snörade på fötterna – bäst att inte smutsa ner finmattorna...\n";
            intro += "\n--------------------------------------------------\n\n";

            intro += _spelare.NuvarandeRum.VisaBeskrivning();

            return intro;
        }

        public string HanteraInmatning(ConsoleKey tangent)
        {
            string output = "";

            // Om vi är i föremålsvalsläge, hantera det först
            if (_isSelectingItem)
            {
                return HanteraForemalsvalet(tangent);
            }

            if (_isInventoryOpen)
            {
                return _spelare.Ryggsack.HanteraInmatning(tangent, _spelare, _story, () => _isInventoryOpen = false);
            }

            if (_isRoomMenuOpen)
            {
                if (tangent == ConsoleKey.Escape)
                {
                    _isRoomMenuOpen = false;
                    return "Du avslutar underhållet.";
                }

                if (tangent >= ConsoleKey.D1 && tangent <= ConsoleKey.D9)
                {
                    return _spelare.NuvarandeRum.HanteraUnderhallsval(
                        tangent, _spelare, _story);
                }

                return "";
            }

            switch (tangent)
            {
                case ConsoleKey.I:
                    _isInventoryOpen = true;
                    return "";

                case ConsoleKey.S:
                    output = _spelare.VisaStatus(_story);
                    output += "\n\nTryck på valfri tangent för att återgå till rummet...";
                    break;

                case ConsoleKey.U:
                    output = $"\nDu ser dig noga omkring i {_spelare.NuvarandeRum.Namn}...\n";
                    output += _spelare.NuvarandeRum.UndersokRum(_spelare, _story);
                    break;

                case ConsoleKey.T:
                    var lista = TaUppSak();

                    if (lista.Count == 1)
                    {
                        output = VerkstallUpplockning(lista[0]);
                    }
                    else if (lista.Count == 0)
                    {
                        output = "\nDet finns inget här att plocka upp.";
                    }
                    else
                    {
                        // Öppna valsidan för flera föremål
                        _isSelectingItem = true;
                        _itemsToSelect = lista;
                        SelectedItemIndex = 0;
                        output = "";
                    }
                    break;

                case ConsoleKey.B:
                    output = _spelare.NuvarandeRum.LasLangBeskrivning();
                    break;

                case ConsoleKey.M:
                    output = _spelare.NuvarandeRum.UtforUnderhall(_spelare, _story);
                    _isRoomMenuOpen = _spelare.NuvarandeRum.HarUnderhallsmeny;
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

        private string HanteraForemalsvalet(ConsoleKey tangent)
        {
            switch (tangent)
            {
                case ConsoleKey.UpArrow:
                    if (SelectedItemIndex > 0)
                        SelectedItemIndex--;
                    return "";

                case ConsoleKey.DownArrow:
                    if (SelectedItemIndex < _itemsToSelect.Count - 1)
                        SelectedItemIndex++;
                    return "";

                case ConsoleKey.Enter:
                    var valdSak = _itemsToSelect[SelectedItemIndex];
                    string resultat = VerkstallUpplockning(valdSak);

                    _isSelectingItem = false;
                    _itemsToSelect.Clear();
                    SelectedItemIndex = 0;

                    return resultat;

                case ConsoleKey.Escape:
                    _isSelectingItem = false;
                    _itemsToSelect.Clear();
                    SelectedItemIndex = 0;

                    return _spelare.NuvarandeRum.VisaBeskrivning();

                default:
                    return "";
            }
        }

        private string FlyttaSpelare(string riktning)
        {
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
