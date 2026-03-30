using Stugan.Core;
using Stugan.Modeller;
using static System.Console;

namespace Stugan.Rooms
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

        public bool VisaExitMeddelande(string riktning)
        {
            // Vi hämtar meddelandet från oss själva (detta rum)
            // Vi skickar in riktningen till vår egen Hamta-metod som sköter ToUpper
            string? meddelande = HamtaUtgangsMeddelande(riktning);

            if (!string.IsNullOrEmpty(meddelande))
            {
                Clear();
                WriteLine($"\n{meddelande}");

                // JAWS-vänlig paus
                Thread.Sleep(2000);
                return true;
            }

            return false;
        }

        public void VisaBeskrivning(bool redanRensat = false)
        {
            if (!redanRensat)
            {
                Clear();
            }

            // HÄR ÄR DIN ORIGINALKOD - HELT OFÖRÄNDRAD:
            WriteLine($"{Namn}");

            // 1. Grundbeskrivningen av rummet
            if (!HarBesokts)
            {
                Thread.Sleep(300);
                WriteLine($"{Beskrivning}");
                WriteLine(HamtaUtgangarBeskrivning());
                HarBesokts = true;
            }
            else
            {
                Thread.Sleep(300);
                WriteLine($"Du är {VisaNamn}.");
                WriteLine(HamtaUtgangarBeskrivning());
            }
            VisaSakerIRummet();
        }

        // 2. Hitta saker som ligger framme och går att ta
        // Vi filtrerar bort de som är gömda (ArGomd == true)
        private void VisaSakerIRummet()
        {
            var synligaSaker = SakerIRummet.Where(s => s.KanPlockasUpp && !s.ArGomd).ToList();

            if (synligaSaker.Count > 0)
            {
                WriteLine("Här ser du också:");
                foreach (var sak in synligaSaker)
                {
                    WriteLine(sak.Namn);
                }
            }
        }

        public void LasLangBeskrivning()
        {
            Clear();
            WriteLine($"{VisaNamn}");
            WriteLine(Beskrivning);
            VisaSakerIRummet();
        }

        public virtual void UndersokRum(Spelare spelare, StoryState story)
        {
            // Som standard i basklassen ger vi bara en allmän beskrivning av rummets atmosfär.
            // Inga menyer, inga listor – bara text.
            WriteLine("Det verkar inte vara något som sticker ut just nu vid en snabb anblick.");
        }

        public virtual bool KanGaIn(Spelare s, StoryState story)
        {
            return true; // Standard: Alla får komma in!
        }

        public virtual void ReageraPaHandling(string handling)
        {
            // Som standard händer absolut ingenting här.
        }

        // I Rum.cs
        public virtual void UtforUnderhall(Spelare spelare, StoryState story)
        {
            // Som standard händer ingenting speciellt.
            WriteLine("\nDet finns inget här som behöver underhållas just nu.");
        }

        // Denna metod är fast och körs likadant för alla rum, 
        // vilket gör att vi slipper rums-specifika utskällningar när vi bara vill se objekten.
        public void VisaObjektLista(Spelare spelare, Core.StoryState story)
        {
            // Vi filtrerar bort gömda saker direkt
            var saker = SakerIRummet.Where(s => !s.ArGomd).ToList();

            if (saker.Count == 0)
            {
                WriteLine("\nDet finns inget särskilt att undersöka här.");
                Thread.Sleep(1000);
                return;
            }

            Clear();
            WriteLine($"--- OBJEKT I {Namn.ToUpper()} ---");
            WriteLine("Vad vill du titta närmare på?");

            for (int i = 0; i < saker.Count; i++)
            {
                WriteLine($"{i + 1}. {saker[i].Namn}");
            }
            WriteLine($"{saker.Count + 1}. Gå tillbaka");

            // Läs in valet
            var info = ReadKey(true);
            if (int.TryParse(info.KeyChar.ToString(), out int index) && index >= 1 && index <= saker.Count)
            {
                var valdSak = saker[index - 1];

                Clear();
                WriteLine($"--- {valdSak.Namn.ToUpper()} ---");
                WriteLine(valdSak.Beskrivning);

                if (valdSak.KanPlockasUpp && !spelare.Ryggsack.HarForemal(valdSak.Namn))
                {
                    WriteLine($"\n(Du kan försöka plocka upp {valdSak.Namn.ToLower()} om du vill.)");
                }

                WriteLine("\nTryck på valfri tangent för att fortsätta...");
                ReadKey(true);

                // Visa rumsbeskrivningen igen så man hittar tillbaka
                VisaBeskrivning(false);
            }
        }
    }
}
