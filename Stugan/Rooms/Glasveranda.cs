using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan.Rooms
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
            // Innan spelaren kliver in på verandan, kollar vi var han kommer ifrån
            if (s.NuvarandeRum is Vardagsrum v)
            {
                // Vi använder den befintliga signalen vi byggde förut!
                v.ReageraPaHandling("vadra");
            }

            return base.KanGaIn(s, story);
        }
    }
}
