using Tjanaren.Core;
using Tjanaren.Modeller;

namespace Tjanaren.Rooms
{
    public class Rum
    {
        public string Namn { get; set; }

        public string VisaNamn { get; set; }

        public string Beskrivning { get; set; }

        public bool HarBesokts { get; set; }

        public bool HarUndersokts { get; set; } = false;

        public List<Spelsak> SakerIRummet { get; set; } = new List<Spelsak>();

        public Position? Plats { get; set; }

        public Dictionary<string, Rum> Utgangar { get; set; } = new Dictionary<string, Rum>();

        public Dictionary<string, string> UtgangsMeddelanden { get; set; } = new Dictionary<string, string>();

        public Rum Norr { get => HamtaUtgang("Norr")!; set => Koppla("Norr", value); }

        public Rum Soder { get => HamtaUtgang("Soder")!; set => Koppla("Soder", value); }

        public Rum Oster { get => HamtaUtgang("Oster")!; set => Koppla("Oster", value); }

        public Rum Vaster { get => HamtaUtgang("Vaster")!; set => Koppla("Vaster", value); }

        public Rum(string namn, string beskrivning, bool harundersokts = false)
        {
            Namn = namn;
            VisaNamn = namn;
            Beskrivning = beskrivning;
            HarUndersokts = harundersokts;
        }

        public void OppnaUtgangar(params string[] riktningar)
        {
            foreach (var riktning in riktningar)
            {
                Utgangar[riktning] = null;
            }
        }

        public string HamtaUtgangarBeskrivning()
        {
            if (Utgangar == null || Utgangar.Count == 0)
            {
                return "Det verkar inte finnas några synliga utgångar härifrån.";
            }

            // Vi mappar dina interna namn till mer naturligt språk
            var utgangsNamn = Utgangar.Keys.Select(u => u switch
            {
                "Norr" => "framåt",
                "Soder" => "bakåt",
                "Oster" => "höger",
                "Vaster" => "vänster",
                "Upp" => "uppåt",
                "Ner" => "nedåt",
                _ => u.ToLower()
            });

            return "Tillgängliga utgångar: " + string.Join(", ", utgangsNamn) + ".";
        }

        public string? HamtaUtgangsMeddelande(string riktning)
        {
            if (UtgangsMeddelanden.TryGetValue(riktning, out string? meddelande))
            {
                return meddelande;
            }
            return null;
        }

        public void Koppla(string riktning, Rum nastaRum)
        {
            Utgangar[riktning] = nastaRum;
        }

        private Rum? HamtaUtgang(string riktning)
        {
            return Utgangar.TryGetValue(riktning, out var rum) ? rum : null;
        }

        public bool VisaExitMeddelande(string riktning, out string exitText)
        {
            exitText = ""; // Vi börjar med en tom text
            string? meddelande = HamtaUtgangsMeddelande(riktning);

            if (!string.IsNullOrEmpty(meddelande))
            {
                // Istället för Clear och WriteLine, sparar vi texten i vår "ut-variabel"
                exitText = $"\n{meddelande}\n";

                // Vi skippar Thread.Sleep här eftersom fönstret inte ska låsa sig
                return true;
            }

            return false;
        }

        public string VisaBeskrivning(bool redanRensat = false)
        {
            // Vi skapar en variabel för att samla upp all text
            string output = "";

            // Istället för Clear() lägger vi till en tom rad om det behövs, 
            // men i WinForms sköter fönstret oftast rensningen själv.
            output += $"{Namn}\n";

            // 1. Grundbeskrivningen av rummet
            if (!HarBesokts)
            {
                // Vi skippar Thread.Sleep(300) – det behövs inte i fönstret 
                // och skulle bara få programmet att se ut som om det "hängt sig"
                output += $"{Beskrivning}\n";
                output += HamtaUtgangarBeskrivning() + "\n";
                HarBesokts = true;
            }
            else
            {
                output += $"Du är {VisaNamn}.\n";
                output += HamtaUtgangarBeskrivning() + "\n";
            }

            // Vi anropar VisaSakerIRummet och lägger till dess text i vår output
            output += VisaSakerIRummet();

            return output;
        }

        // 2. Hitta saker som ligger framme och går att ta
        // Vi filtrerar bort de som är gömda (ArGomd == true)
        public string VisaSakerIRummet()
        {
            var synligaSaker = SakerIRummet.Where(s => s.KanPlockasUpp && !s.ArGomd).ToList();
            string text = "";

            if (synligaSaker.Count > 0)
            {
                // Vi lägger till en extra radbrytning i början för att få luft i fönstret
                text += "\nHär ser du också:\n";
                foreach (var sak in synligaSaker)
                {
                    text += $"{sak.Namn}\n";
                }
            }

            return text;
        }

        public string LasLangBeskrivning()
        {
            // Vi byter ut Clear() mot att börja på en fräsch sträng
            string output = $"{VisaNamn}\n";

            output += $"{Beskrivning}\n";

            // Vi lägger till utgångarna också, så man inte går vilse efter att ha läst
            output += HamtaUtgangarBeskrivning() + "\n";

            // Och så anropar vi den nu sträng-returnerande VisaSakerIRummet()
            output += VisaSakerIRummet();

            return output;
        }

        public virtual string UndersokRum(Spelare spelare, StoryState story)
        {
            // Vi returnerar bara texten istället för att köra WriteLine.
            // Inga menyer, inga listor – bara ren text som skickas till motorn.
            return "Det verkar inte vara något som sticker ut just nu vid en snabb anblick.";
        }

        public virtual bool KanGaIn(Spelare s, StoryState story)
        {
            return true; // Standard: Alla får komma in!
        }

        public virtual string ReageraPaHandling(string handling)
        {
            // Som standard händer absolut ingenting här, 
            // så vi returnerar en tom sträng eller ett neutralt besked.
            return "";
        }

        // I Rum.cs
        public virtual string UtforUnderhall(Spelare spelare, StoryState story)
        {
            // Vi returnerar standardtexten istället för att skriva ut den
            return "\nDet finns inget här som behöver underhållas just nu.";
        }

        // Denna metod är fast och körs likadant för alla rum, 
        // vilket gör att vi slipper rums-specifika utskällningar när vi bara vill se objekten.
        public List<Spelsak> VisaObjektLista()
        {
            // Vi filtrerar bara fram det som faktiskt syns
            return SakerIRummet.Where(s => !s.ArGomd).ToList();
        }

        // En ny hjälpmetod som ger beskrivningen av en specifik sak när man valt den i listan
        public string UndersokValdSak(Spelsak valdSak, Spelare spelare)
        {
            if (valdSak == null) return "";

            string output = $"--- {valdSak.Namn.ToUpper()} ---\n";
            output += $"{valdSak.Beskrivning}\n";

            if (valdSak.KanPlockasUpp && !spelare.Ryggsack.HarForemal(valdSak.Namn))
            {
                output += $"\n(Du kan försöka plocka upp {valdSak.Namn.ToLower()} om du vill.)";
            }

            return output;
        }
    }
}
