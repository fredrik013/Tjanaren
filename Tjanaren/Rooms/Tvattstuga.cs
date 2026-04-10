using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Tvattstuga : Rum
    {
        public Tvattstuga() : base("Tvättstugan", "Du har kommit in i en tvättstuga. Här råder det god ordning och det luktar starkt av tvättmedel.Här finns det både tvättmaskin och torktumlare..")
        {
            VisaNamn = "i tvättstugan";
            OppnaUtgangar("Oster");

            SakerIRummet.Add(new Inredning("Tvättmaskin", "En modern tvättmaskin som ser ut att vara i gott skick. Den är fylld med smutsiga kläder som väntar på att bli rena.", false, false));
            SakerIRummet.Add(new Inredning("Torktumlare", "En torktumlare som matchar tvättmaskinen."));

        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            string output = "";

            // Vi kollar bootsen först eftersom de ofta är smutsiga efter trädgården
            if (spelare.Ryggsack.HarForemal("Boots"))
            {
                output += "\nDu tar fram borsten och putsar bort leran från dina boots.\n";
                output += "De glänser nu som nya.\n";
            }

            // Sedan kollar vi maskinen
            if (!story.TvattmaskinArIgang)
            {
                story.TvattmaskinArIgang = true;
                output += "\n[KLICK-SURRRRR]\n";
                output += "Du trycker på startknappen. Maskinen låser luckan och börjar snurra.\n";
                output += "Vattnet väser i rören och tvättcykeln är igång.\n";
            }
            else
            {
                output += "\nTvättmaskinen hummar redan troget. Den verkar vara mitt i ett program.\n";
            }

            return output;
        }
    }
}
