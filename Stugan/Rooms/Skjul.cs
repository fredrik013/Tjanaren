using Stugan.Modeller.Saker;

namespace Stugan.Rooms
{
    public class Skjul : Rum
    {
        public Skjul() : base("Skjulet", "Du kliver in i skjulet. Det är trångt och mörkt, och doften av gammalt trä och olja fyller luften. " +
            "Runt omkring dig står verktyg och trädgårdsredskap staplade på hyllor.")
        {
            VisaNamn = "i skjulet";
            OppnaUtgangar("Vaster");
            SakerIRummet.Add(new Klader("Gummistövlar", "Ett par gamla gummistövlar som har sett bättre dagar men de skyddar nog bra mot vatten.", Bekladnadstyp.Skydd, Kroppsdel.Fot, BekladnadsLager.Mellan, true, true, false));
        }
    }
}
