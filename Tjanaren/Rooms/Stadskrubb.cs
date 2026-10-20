using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;
namespace Tjanaren.Rooms
{
    public class Stadskrubb : Rum
    {
        public Stadskrubb() : base("Städskrubb", "Du står i en liten städskrubb. Det är trångt här inne, men du kan skymta några gamla föremål som ligger huller om buller på golvet. " +
            "Du lägger märke till en bänk med en ho där man kan fylla på vatten för att rengöra skurmopp med mera. Under bänken finns ett skåp. Det finns en dörr som leder till tvättstugan.")
        {
            VisaNamn = "i städskrubben";
            OppnaUtgangar("Oster");
            SakerIRummet.Add(new Redskap(
                            "Hink",
                            "En hink som antagligen ska användas vid städning.",
                            true,
                            false
                        ));
            SakerIRummet.Add(new Redskap(
                            "Mopp",
                            "En gammal mopp som ser ut att ha sett bättre dagar.",
                            true,
                            false
                        ));
            SakerIRummet.Add(new Redskap(
                            "Sopborste",
                            "En sopborste som ser ut att ha använts flitigt.",
                            true,
                            false
                        ));
            SakerIRummet.Add(new Redskap(
                            "Dammsugare",
                            "En dammsugare som kan användas för att dammsuga de fina mattorna med.",
                            true,
                            false
                        ));
        }
    }
}
