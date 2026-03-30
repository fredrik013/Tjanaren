using Stugan.Modeller.Saker;
using Stugan.Rooms;
using static System.Console;

namespace Stugan.Core
{
    public class Spelare
    {
        public Rum NuvarandeRum { get; set; }

        public Position? Position => NuvarandeRum.Plats;

        public Inventory Ryggsack { get; private set; }

        public string AktivtSkodon { get; set; } // Vanlig egenskap så den kan ändras!

        public bool ArSkadad { get; set; }

        public Spelare(Rum startRum)
        {
            NuvarandeRum = startRum;
            Ryggsack = new Inventory();
            Ryggsack.LaggTill(new Klader(
                    "Boots",
                    "Dina rejäla, leriga kängor.",
                    Bekladnadstyp.Ute, Kroppsdel.Fot, BekladnadsLager.Mellan,
                    true,  // Skyddar mot vatten
                    true,  // Kan plockas upp
                    false, // Inte gömda
                    "Du snörar på dig de tunga bootsen.",
                    "Du sparkar av dig bootsen med en lättad suck.")
            { ArAktiv = true }); // Den här lilla måsvingen sätter egenskapen direkt!
            AktivtSkodon = Bekladnadstyp.Ute.ToString();

            Ryggsack.LaggTill(new Klader(
                    "Byxor",
"Ett par blåa men funktionella jeans.",
                    Bekladnadstyp.Plagg, Kroppsdel.Nederdel, BekladnadsLager.Mellan,
                    false,
                    true,
                    false,
"Du kränger på dig jeansen.",
                    "Du kränger av dig jeansen."
                )
            { ArAktiv = true });

            Ryggsack.LaggTill(new Klader(
        "T-shirt",
"En svart t-shirt med ett slitet tryck av ett gammalt rockband.",
        Bekladnadstyp.Plagg, Kroppsdel.Torso, BekladnadsLager.Underst,
        false,
        true,
        false,
"Du drar på dig t-shirten och känner dig genast lite mer bekväm.",
        "Du tar av dig t-shirten och känner dig lite mer utsatt."
    )
            { ArAktiv = true });



            ArSkadad = false;
        }

        public string AktivtSkodonNamn
        {
            get
            {
                var skodon = Ryggsack.GetAllaSaker()
                    .OfType<Klader>()
                    .FirstOrDefault(k => k.ArAktiv && (k.Typ == Bekladnadstyp.Inne || k.Typ == Bekladnadstyp.Ute || k.Typ == Bekladnadstyp.Skydd));

                return skodon?.Namn ?? string.Empty;
            }
        }

        public void VisaStatus(StoryState story)
        {
            WriteLine();
            WriteLine("--- DIN STATUS ---");

            // Hälsa
            WriteLine(ArSkadad ? "Mående: Du är skadad och rör dig tungt." : "Mående: Du känner dig pigg och oskadd.");

            // Hitta aktiva kläder i ryggsäcken
            var allaKlader = Ryggsack.GetAllaSaker().OfType<Klader>().Where(k => k.ArAktiv).ToList();

            string huvud = allaKlader.FirstOrDefault(k => k.Placering == Kroppsdel.Huvud)?.Namn ?? "inget";

            string hander = allaKlader.FirstOrDefault(k => k.Placering == Kroppsdel.Hand)?.Namn ?? "inget";

            var kroppsPlagg = allaKlader
                .Where(k => k.Placering == Kroppsdel.Torso || k.Placering == Kroppsdel.Nederdel || k.Placering == Kroppsdel.Helakroppen)
                .Select(k => k.Namn)
                .ToList();

            string kropp = kroppsPlagg.Count > 0 ? string.Join(", ", kroppsPlagg) : "inget";

            var fotPlagg = allaKlader
                    .Where(k => k.Placering == Kroppsdel.Fot)
                    .Select(k => k.Namn)
                    .ToList();

            string fotter = fotPlagg.Count > 0 ? string.Join(", ", fotPlagg) : "inget (barfota)";
            WriteLine($"På huvudet: {huvud}");
            WriteLine($"På händerna: {hander}");
            WriteLine($"På kroppen: {kropp}");
            WriteLine($"På fötterna: {fotter}");

            WriteLine("------------------");
            WriteLine($"Uppdrag: {story.GetStatusBeskrivning()}");
            WriteLine("------------------");
            WriteLine();
        }
    }
}
