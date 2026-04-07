using Forvaltaren.Core;
using Forvaltaren.Modeller.Saker;

namespace Forvaltaren.Rooms
{
    public class Pannrum : Rum
    {
        public Pannrum() : base("Pannrummet", "Du har kommit in i ett pannrum. Här står det decimeterdjupt med vatten." + "Du ser direkt att det kommer från ett läckande rör.", false)
        {
            VisaNamn = "i pannrummet";
            OppnaUtgangar("Vaster");
            SakerIRummet.Add(new AllmanSak("Rör", "Ett läckande rör. Du måste ha ett verktyg för att kunna laga det.", false, false));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi hämtar de aktiva skyddskläderna
            var aktivaSkydd = s.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .Where(k => k.ArAktiv && k.Typ == Bekladnadstyp.Skydd)
                .ToList();

            // 1. "Naken-kontrollen"
            if (!aktivaSkydd.Any())
            {
                // Ingen text här längre – motorn sköter hindret
                return false;
            }

            // 2. Switchen kollar vad som saknas
            switch (true)
            {
                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Fot):
                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Helakroppen):
                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Hand):
                    return false; // Stopp direkt om något saknas

                default:
                    return true; // Allt på plats!
            }
        }

        public override string UndersokRum(Spelare spelare, StoryState story)
        {
            string output = "";

            if (story.RorArLagat)
            {
                // Den "belönande" beskrivningen
                output += "\nLuften i pannrummet är nu klar och torr. Det lagade röret blänker svagt i ljuset.\n";
                output += "Det sjuder hemtrevligt från pannan och värmen sprider sig i rören.\n";
                output += "Här finns inget mer som behöver underhållas just nu.\n";
            }
            else
            {
                // Hintens beskrivning (innan lagning)
                output += "\nDet läckande röret sprutar het ånga rätt ut i rummet.\n";
                output += "Det är uppenbart att anläggningen är i desperat behov av **underhåll** (M).\n";
                output += "Muttern sitter alldeles för hårt för att dras åt med bara händerna.\n";
            }

            return output;
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            string output = "";

            // Om röret redan är lagat
            if (story.RorArLagat)
            {
                return "\nDu ser det lagade röret. Det ser torrt och säkert ut nu.";
            }

            // Vi kollar om spelaren har en AKTIV rörtång
            var aktivRortang = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>()
                .FirstOrDefault(r => r.Namn == "Rörtång" && r.ArAktiv);

            if (aktivRortang != null)
            {
                // Framgång!
                story.RorArLagat = true;

                // Vi uppdaterar rumsbeskrivningen permanent
                Beskrivning = "Ett nu tyst och varmt pannrum. Röret är lagat och ångan har lagt sig.";

                output += "\nDu placerar rörtången runt den lösa kopplingen och tar i allt vad du orkar.\n";
                output += "Med ett metalliskt knarrande ger muttern med sig och dras åt.\n";
                output += "Det sista pysandet dör ut. Röret är lagat!\n";
            }
            else
            {
                // Spelaren har inte rörtången aktiv
                bool harRortangMenInaktiv = spelare.Ryggsack.GetAllaSaker().Any(s => s.Namn == "Rörtång");

                if (harRortangMenInaktiv)
                {
                    output += "\nDet sprutar ånga från en koppling som verkar sitta helt lös.\n";
                    output += "Du har visserligen en rörtång i ryggsäcken, men den gör ingen nytta där.\n";
                    output += "Du behöver nog ta fram den (Gör den aktiv) om du ska kunna dra åt muttern.\n";
                }
                else
                {
                    output += "\nDet läckande röret sprutar het ånga rätt ut i rummet.\n";
                    output += "Muttern sitter alldeles för hårt för att dras åt med fingrarna.\n";
                    output += "Du behöver ett rejält verktyg – som en rörtång – för att få stopp på läckan.\n";
                }
            }

            return output;
        }
    }
}
