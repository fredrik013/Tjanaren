using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Glasveranda : Rum
    {
        public Glasveranda() : base("Glasverandan", "Du öppnar dörren mot den inbjudande glasverandan. Här ser man ett möblemang med sköna rottingstolar som passar finfolket." + "i fönstren står det välskötta blommor. Under bordet ligger det en matta där man definitivt inte får sätta smutsiga fötter.")
        {
            VisaNamn = "på glasverandan";
            OppnaUtgangar("Soder", "Oster");

            SakerIRummet.Add(new AllmanSak("Minneslapp",
                "Lappen är skriven med en nästan provocerande prydlig handstil: \n" +
                "'Till den som eventuellt har gått vilse i mitt hus: \n" +
                "Skulle källarrören få för sig att protestera igen, vänligen konsultera läskamraten. \n" +
                "Och kom ihåg – blommorna i hallen behöver mer än bara vatten, de vaktar även ingångar.'",
                true)); // true betyder att den går att plocka upp om man vill
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Kontrollera om spelaren kommer från vardagsrummet
            if (s.NuvarandeRum is Vardagsrum v)
            {
                string reaktion = v.ReageraPaHandling("vadra");

                if (!string.IsNullOrWhiteSpace(reaktion))
                {
                    MessageBox.Show(
                        reaktion,
                        "Vädring",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            return base.KanGaIn(s, story);
        }
    }
}
