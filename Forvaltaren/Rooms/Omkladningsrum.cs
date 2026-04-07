using Forvaltaren.Core;
using Forvaltaren.Modeller.Saker;
using static System.Console;

namespace Forvaltaren.Rooms
{
    public class Omkladningsrum : Rum
    {
        bool dorrLast = true;
        public Omkladningsrum() : base("Omklädningsrummet", "Du har kommit in i ett omklädningsrum. Det är här arbetarna byter om. Runt väggarna finns det bänkar")
        {
            VisaNamn = "i omklädningsrummet";
            OppnaUtgangar("Vaster");
            SakerIRummet.Add(new Klader("Overall", "Det här verkar vara en skyddsoverall av något slag. Den ser ut att tåla vatten.", Bekladnadstyp.Skydd, Kroppsdel.Helakroppen, BekladnadsLager.Ytterst, true, true, false));
            SakerIRummet.Add(new Klader(
    "Arbetshandskar",
    "Ett par kraftiga gummihandskar som ser ut att tåla både fukt och smuts.",
    Bekladnadstyp.Skydd,
    Kroppsdel.Hand,
    BekladnadsLager.Ytterst,
    true, // Vattentäta
    true,
    false
));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            var aktivNyckel = s.Ryggsack.GetAllaSaker()
    .OfType<AllmanSak>()
    .FirstOrDefault(a => a.Namn == "Nyckel" && a.ArAktiv);

            if (!dorrLast) return true;
            if (aktivNyckel != null)
            {
                WriteLine("\nDu sticker in den rostiga nyckeln i låset. Det gnisslar till, men dörren går upp!");
                WriteLine("Tryck på valfri tangent för att gå in...");
                ReadKey(true);
                dorrLast = false;
                return true;
            }

            else
            {
                WriteLine("Dörren är låst. Du måste ha en nyckel för att komma in.");
                return false;
            }
        }
    }
}
