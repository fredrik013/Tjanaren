using Stugan.Modeller.Saker;
namespace Stugan.Rooms
{
    public class Gardsplan : Rum
    {
        public Gardsplan() : base("Gårdsplanen", "Du står på en grusad gårdsplan. Den röda stugan med sina vita knutar ser inbjudande ut i solskenet. " +
        "Rakt framför dig, åt norr, leder en gammal trädörr in till huset." + "Till höger skymtar man ett gammalt träskjul och till vänster skymtar man en grind.")
        {
            VisaNamn = "på gårdsplanen";
            OppnaUtgangar("Norr", "Vaster", "Oster");
            SakerIRummet.Add(new Redskap(
                            "Spade",
                            "En rostig men rejäl spade.",
                            true,
                            false
                        ));
        }


    }
}
