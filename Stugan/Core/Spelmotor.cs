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
            // Starta spelaren i Hallen (Position 0,0,0)
            _spelare = new Spelare(_worldMap.HamtaRum(new Position(0, 0, 0)));
        }

        public string HamtaStartText()
        {
            return "Du står utanför stugan. Regnet piskar mot ansiktet.";
        }

        public string HamtaInventoryTest()
        {
            return "I din ryggsäck hittar du: Ett par boots.";
        }

        public string HanteraInmatning(ConsoleKey tangent)
        {
            // Vi skapar en variabel för att samla upp texten som ska visas
            string output = "";

            switch (tangent)
            {
                case ConsoleKey.I:
                    // Här behöver Ryggsack.Visa returnera en string i framtiden
                    output = "Du tittar i ryggsäcken...\n";
                    output += _spelare.NuvarandeRum.VisaBeskrivning();
                    break;

                case ConsoleKey.S:
                    output = _spelare.VisaStatus(_story);
                    output += "\n(Tryck på valfri tangent för att återgå)";
                    break;

                case ConsoleKey.UpArrow:
                    output = FlyttaSpelare("Norr");
                    break;

                // ... och så vidare för alla dina case ...

                default:
                    output = "Den tangenten gör inget just nu.";
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
