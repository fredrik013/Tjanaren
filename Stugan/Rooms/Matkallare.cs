using Stugan.Modeller.Saker;

namespace Stugan.Rooms
{
    public class Matkallare : Rum
    {
        public Matkallare() : base("Matkällaren", "Du är i en matkällare. Det är kallt och fuktigt här. Det finns en dörr som leder ut.")
        {
            VisaNamn = "i matkällaren";
            OppnaUtgangar("Soder");

            SakerIRummet.Add(new Livsmedel("En whiskyflaska", "En flaska Jameson som ser väldigt dyr ut.", Livsmedelstyp.Sprit)
            { Anvandningsmeddelande = "Du klunkar i dig whiskyflaskan och känner dig på mycket bra humör. Nu jäklar ska världsproblemen lösas! Skål!" });
        }
    }
}
