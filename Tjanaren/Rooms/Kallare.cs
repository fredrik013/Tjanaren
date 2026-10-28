using Tjanaren.Core;
using Tjanaren.Modeller.Saker;
using static System.Console;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Tjanaren.Rooms
{
    public class Kallare : Rum
    {
        public Kallare() : base("Källaren",
"Luften här nere är kall och luktar fuktig jord. Högt uppe på väggen sitter en hylla." +
                    "Ett släpande ljud hörs inifrån mörkret. Du hör också ljudet av forsande vatten.")
        {
            VisaNamn = "i källaren";
            OppnaUtgangar("Upp", "Soder", "Norr", "Oster", "Vaster");

            SakerIRummet.Add(new Klader("Plasttofflor", "Ett par blå plasttofflor. De ser ut att tåla både fukt och smuts.", Bekladnadstyp.Skydd, Kroppsdel.Fot, BekladnadsLager.Mellan, true, true, false));
            SakerIRummet.Add(new Redskap(
                "Hink",
                "En hink som antagligen ska användas vid städning.",
arAktiv: true
            ));
            SakerIRummet.Add(new Redskap(
                            "Mopp",
                            "En gammal mopp som ser ut att ha sett bättre dagar.",
                            arAktiv: true
                        ));
        }


        public override bool KanGaIn(Spelare s, StoryState story)
        {
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                MessageBox.Show(
                    "Dånandet från dina tunga steg i trappan ekar fortfarande mellan källarväggarna.\n" +
                    "Det fuktiga betonggolvet här nere verkar nästan vibrera av oväsendet.",
                    "Källaren",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Du smyger ner för trappan och landar mjukt på det kalla källargolvet.\n" +
                    "Dina steg är nästan ljudlösa i tystnaden häruppe.",
                    "Källaren",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            // Spelaren får alltid komma in i källaren
            return true;
        }

        public override string UndersokRum(Spelare spelare, StoryState story)
        {
            string output = "";
            var tofflor = SakerIRummet.Find(s => s.Namn.ToLower().Contains("tofflor"));

            // 1. Hitta plagget (vi kollar vad som faktiskt sitter på fötterna)
            var p = spelare.Ryggsack.GetAllaSaker()
                .OfType<Klader>()
                .FirstOrDefault(p => p.ArAktiv && (p.Typ == Bekladnadstyp.Ute || p.Typ == Bekladnadstyp.Inne));

            if (story.HeltRentIKallaren)
                return "\nKällargolvet är rent och torrt. Inte ett spår av den tidigare översvämningen finns kvar.\n";

            // 2. Switchen som bygger upp texten baserat på fotbeklädnad
            switch (p)
            {
                case null:
                    // Spelaren är barfota
                    output += "\nDet iskalla källarvattnet klafsar obehagligt mellan tårna. Du blir dyngsur!\n";
                    break;

                case var k when !k.SkyddarMotVatten:
                    // Har skor, men de läcker
                    string ljudLäck = k.Typ == Bekladnadstyp.Ute ? "dunsar" : "ploppar";
                    output += $"\nDina skor {ljudLäck} i vätan, men fukten tränger igenom. De skyddar inte mot vatten!\n";
                    break;

                case var k when k.SkyddarMotVatten:
                    // Har skor och de är täta
                    string ljudTät = k.Typ == Bekladnadstyp.Ute ? "dunsar tungt" : "ploppar hemtrevligt";
                    output += $"\nDina skor {ljudTät} mot betongen och håller dina fötter torra.\n";
                    break;
            }

            // Retur till motorn så JAWS får något att bita i
            return output;
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            var mopp = spelare.Ryggsack.GetAllaSaker()
     .OfType<Redskap>()
     .FirstOrDefault(r => r.Namn == "Mopp" && r.ArAktiv);

            var hink = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>()
                .FirstOrDefault(r => r.Namn == "Hink" && r.ArAktiv);

            var vattentaligaSkor = spelare.Ryggsack.GetAllaSaker()
    .OfType<Klader>()
    .FirstOrDefault(k => k.ArAktiv && k.SkyddarMotVatten);

            switch (true)
            {
                case true when story.HeltRentIKallaren:
                    return "\nDu ser dig omkring och konstaterar att källaren redan är ordentligt städad. Här finns inget mer att göra.";

                case true when !story.RorArLagat:
                    return "\nDet forsande vattnet gör det omöjligt att städa ordentligt. Du måste åtgärda problemet först.";

                case true when !story.SkurvattenKlart:
                    return "\nDu ser dig omkring. Det här kräver en ordentlig rengöring, men du har ännu inte gjort alla förberedelser.";

                case true when vattentaligaSkor == null:
                    return "\nDu behöver lämpliga skor som tål vatten för det här uppdraget.";

                case true when hink == null:
                    return "\nDu behöver något att ha rengöringsvattnet i. Se också till att ha utrustningen framme när du börjar.";

                case true when mopp == null:
                    return "\nDet här golvet blir inte rent av sig självt. Du behöver något lämpligt att rengöra det med, och du måste ha det framme.";

                default:
                    story.HeltRentIKallaren = true;
                    mopp.ArRen = false;

                    return "\nDu tar itu med det omfattande städarbetet. " +
                           "Det smutsiga vattnet skvalpar medan du arbetar dig fram över källargolvet.\n" +
                           "Du skrubbar, skurar och kämpar dig vidare in i pannrummet, " +
                           "där resterna efter vattenläckan fortfarande gör sig påminda.\n" +
                           "Efter ett rejält arbetspass är både källaren och pannrummet äntligen rena.\n" +
                           "Du betraktar resultatet med tillfredsställelse. " +
                           "Moppen däremot har sett betydligt bättre dagar.";
            }

        }
    }
}