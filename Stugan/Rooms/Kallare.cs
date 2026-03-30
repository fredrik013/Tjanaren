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

        public override string UndersokRum(Spelare spelare, Core.StoryState story)
        {
            string output = "";
            var tofflor = SakerIRummet.Find(s => s.Namn.ToLower().Contains("tofflor"));

            // 1. Hitta plagget (vi kollar vad som faktiskt sitter på fötterna)
            var p = spelare.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .FirstOrDefault(p => p.ArAktiv && (p.Typ == Bekladnadstyp.Ute || p.Typ == Bekladnadstyp.Inne));

            // 2. Switchen som bygger upp texten baserat på fotbeklädnad
            switch (p)
            {
                case null:
                    // Spelaren är barfota
                    output += "\nDet iskalla källarvattnet klafsar obehagligt mellan tårna. Du blir dyngsur!\n";
                    break;

                case var k when !k.SkyddarMotVatten:
                    // Har skor, men de läcker
                    string ljudLäck = (k.Typ == Bekladnadstyp.Ute) ? "dunsar" : "ploppar";
                    output += $"\nDina skor {ljudLäck} i vätan, men fukten tränger igenom. De skyddar inte mot vatten!\n";
                    break;

                case var k when k.SkyddarMotVatten:
                    // Har skor och de är täta
                    string ljudTät = (k.Typ == Bekladnadstyp.Ute) ? "dunsar tungt" : "ploppar hemtrevligt";
                    output += $"\nDina skor {ljudTät} mot betongen och håller dina fötter torra.\n";
                    break;
            }

            // 3. Själva sökandet efter tofflorna på hyllan
            if (!HarUndersokts)
            {
                output += "\nDu undersöker den torra hyllan högt upp på väggen.\n";
                output += "Dina fingrar nuddar något mjukt... det är ett par innetofflor!\n";

                if (tofflor != null) tofflor.ArGomd = false;
                HarUndersokts = true;
                output += "\nEtt svagt, belåtet mumlande hörs från mörkret.\n";
            }
            else if (tofflor != null && !tofflor.ArGomd)
            {
                output += "\nDu ser innetofflorna ligga på hyllan där du hittade dem.\n";
            }
            else
            {
                output += "\nDu letar noga på hyllorna men hittar inget mer än damm och spindelväv.\n";
            }

            // Retur till motorn så JAWS får något att bita i
            return output;
        }
    }
}