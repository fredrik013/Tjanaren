using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
{
    public class Kallare : Rum
    {
        public Kallare() : base("Källaren",
"Luften här nere är kall och luktar fuktig jord. " +
                    "Ett släpande ljud hörs inifrån mörkret. Du hör också ljudet av forsande vatten.")
        {
            VisaNamn = "i källaren";
            OppnaUtgangar("Upp", "Soder", "Norr", "Oster", "Vaster");
            SakerIRummet.Add(new Klader("Innetofflor", "Mjuka innetofflor för fina mattor.", Bekladnadstyp.Inne, Kroppsdel.Fot, BekladnadsLager.Mellan, false, true, true, "Du tar på dig tofflorna. Nu behöver du inte frysa om fötterna och finfolket kan inte klaga på några smutsiga skor.", "Du tar av dig tofflorna."));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi använder din snygga property för att ge feedback direkt vid ankomst
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                WriteLine("\nDånandet från dina tunga steg i trappan ekar fortfarande mellan källarväggarna.");
                WriteLine("Det fuktiga betonggolvet här nere verkar nästan vibrera av oväsendet.");
            }
            else
            {
                WriteLine("\nDu smyger ner för trappan och landar mjukt på det kalla källargolvet.");
                WriteLine("Dina steg är nästan ljudlösa i tystnaden häruppe.");
            }

            // Vi släpper alltid in dem, men nu har de fått veta att de hörs!
            return true;
        }

        public override void UndersokRum(Spelare spelare, Core.StoryState story)
        {
            var rum = spelare.NuvarandeRum;

            var tofflor = SakerIRummet.Find(s => s.Namn.ToLower().Contains("tofflor"));

            // 1. Hitta plagget (utan ToLower-slarv)
            var p = spelare.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .FirstOrDefault(p => p.ArAktiv && (p.Typ == Bekladnadstyp.Ute || p.Typ == Bekladnadstyp.Inne));
            // 2. Switchen som sköter allt
            switch (p)
            {
                case null:
                    // Spelaren är barfota
                    WriteLine("\nDet iskalla källarvattnet klafsar obehagligt mellan tårna. Du blir dyngsur!");
                    break;

                case var k when !k.SkyddarMotVatten:
                    // Har skor, men de läcker (Använder Enum istället för sträng)
                    string ljudLäck = (k.Typ == Bekladnadstyp.Ute) ? "dunsar" : "ploppar";
                    WriteLine($"\nDina skor {ljudLäck} i vätan, men fukten tränger igenom. De skyddar inte mot vatten!");
                    break;

                case var k when k.SkyddarMotVatten:
                    // Har skor och de är täta (Använder Enum istället för sträng)
                    string ljudTät = (k.Typ == Bekladnadstyp.Ute) ? "dunsar tungt" : "ploppar hemtrevligt";
                    WriteLine($"\nDina skor {ljudTät} mot betongen och håller dina fötter torra.");
                    break;
            }
            // 2. Själva sökandet
            if (!HarUndersokts)
            {
                WriteLine("\nDu undersöker den torra hyllan högt upp på väggen.");
                WriteLine("Dina fingrar nuddar något mjukt... det är ett par innetofflor!");

                tofflor.ArGomd = false;
                HarUndersokts = true;
                WriteLine("\nEtt svagt, belåtet mumlande hörs från mörkret.");
            }
            else if (tofflor != null && !tofflor.ArGomd)
            {
                WriteLine("\nDu ser innetofflorna ligga på hyllan där du hittade dem.");
            }
            else
            {
                WriteLine("\nDu letar noga på hyllorna men hittar inget mer än damm och spindelväv.");
            }
        }
    }
}