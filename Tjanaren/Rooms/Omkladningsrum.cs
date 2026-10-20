using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
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
            SakerIRummet.Add(new Klader("Plasttofflor", "Ett par blå plasttofflor. De ser ut att tåla både fukt och smuts.", Bekladnadstyp.Skydd, Kroppsdel.Fot, BekladnadsLager.Mellan, true, true, false));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            var aktivNyckel = s.Ryggsack.GetAllaSaker()
    .OfType<AllmanSak>()
    .FirstOrDefault(a => a.Namn == "Nyckel" && a.ArAktiv);

            if (!dorrLast) return true;
            if (aktivNyckel != null)
            {
                string text = "\nDu sticker in den rostiga nyckeln i låset. Det gnisslar till, men dörren går upp!";
                MessageBox.Show(text, "Dörren låses upp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dorrLast = false;
                return true;
            }

            else
            {
                string text = "Dörren är låst. Du måste ha en nyckel för att komma in.";
                MessageBox.Show(text, "Dörren är låst", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}
