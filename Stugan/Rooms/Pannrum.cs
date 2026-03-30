using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
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
            // Vi hämtar ALLA aktiva skyddskläder en gång för alla
            var aktivaSkydd = s.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .Where(k => k.ArAktiv && k.Typ == Bekladnadstyp.Skydd)
                .ToList();

            // 1. "Naken-kontrollen" - har man inget skydd alls på sig?
            if (!aktivaSkydd.Any())
            {
                WriteLine("Det ser farligt ut där inne. Du behöver nog någon form av skyddsutrustning.");
                return false;
            }

            // 2. Switchen kollar nu bara VAD som saknas i listan
            switch (true)
            {
                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Fot):
                    WriteLine("Du kan inte gå in där utan skydd på fötterna, det är alldeles för blött.");
                    return false;

                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Helakroppen):
                    WriteLine("Du behöver skydd för kroppen innan du kliver in.");
                    return false;

                case bool _ when !aktivaSkydd.Any(k => k.Placering == Kroppsdel.Hand):
                    WriteLine("Du kan inte gå in utan skydd för händerna.");
                    return false;

                default:
                    // Om vi har något skydd (if-satsen ovan) och inget saknas i switchen...
                    return true;
            }
        }

        public override void UndersokRum(Spelare spelare, StoryState story)
        {
            if (story.RorArLagat)
            {
                // Den "belönande" beskrivningen
                WriteLine("\nLuften i pannrummet är nu klar och torr. Det lagade röret blänker svagt i ljuset.");
                WriteLine("Det sjuder hemtrevligt från pannan och värmen sprider sig i rören.");
                WriteLine("Här finns inget mer som behöver underhållas just nu.");
            }
            else
            {
                // Hintens beskrivning (innan lagning)
                WriteLine("\nDet läckande röret sprutar het ånga rätt ut i rummet.");
                WriteLine("Det är uppenbart att anläggningen är i desperat behov av **underhåll** (M).");
                WriteLine("Muttern sitter alldeles för hårt för att dras åt med bara händerna.");
            }
        }

        public override void UtforUnderhall(Spelare spelare, StoryState story)
        {
            // Om röret redan är lagat finns det inget mer att undersöka här
            if (story.RorArLagat)
            {
                WriteLine("\nDu ser det lagade röret. Det ser torrt och säkert ut nu.");
                return;
            }

            // Vi kollar om spelaren har en AKTIV rörtång i handen
            var aktivRortang = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>()
                .FirstOrDefault(r => r.Namn == "Rörtång" && r.ArAktiv);

            if (aktivRortang != null)
            {
                // Framgång! Spelaren har rörtången redo.
                story.RorArLagat = true;

                // Vi uppdaterar rumsbeskrivningen permanent
                this.Beskrivning = "Ett nu tyst och varmt pannrum. Röret är lagat och ångan har lagt sig.";

                WriteLine("\nDu placerar rörtången runt den lösa kopplingen och tar i allt vad du orkar.");
                WriteLine("Med ett metalliskt knarrande ger muttern med sig och dras åt.");
                WriteLine("Det sista pysandet dör ut. Röret är lagat!");
            }
            else
            {
                // Spelaren har inte rörtången aktiv
                bool harRortangMenInaktiv = spelare.Ryggsack.GetAllaSaker().Any(s => s.Namn == "Rörtång");

                if (harRortangMenInaktiv)
                {
                    WriteLine("\nDet sprutar ånga från en koppling som verkar sitta helt lös.");
                    WriteLine("Du har visserligen en rörtång i ryggsäcken, men den gör ingen nytta där.");
                    WriteLine("Du behöver nog ta fram den (Gör den aktiv) om du ska kunna dra åt muttern.");
                }
                else
                {
                    WriteLine("\nDet läckande röret sprutar het ånga rätt ut i rummet.");
                    WriteLine("Muttern sitter alldeles för hårt för att dras åt med fingrarna.");
                    WriteLine("Du behöver ett rejält verktyg – som en rörtång – för att få stopp på läckan.");
                }
            }
        }
    }
}
