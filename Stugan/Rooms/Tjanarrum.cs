using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

namespace Stugan.Rooms
{
    public class Tjanarrum : Rum
    {
        public Tjanarrum() : base("Tjänarrummet", "Du har kommit in i tjänarens rum som ligger under källartrappan. Läget speglar verkligen var finfolket anser att tjänarna hör hemma. Rummet är spartanskt inrett med en sliten tältsäng och en garderob som har sett bättre dagar.")
        {
            VisaNamn = "i tjänarrummet";
            OppnaUtgangar("Norr");

            SakerIRummet.Add(new Inredning(
                "Garderob",
                "En tung garderob i ek. Den ser ut att ha stått här sedan huset byggdes."
                            ));

            // Finkläderna som krävs för middagen
            SakerIRummet.Add(new Klader(
                "Skjorta",
                "En vit, nystruken skjorta. Den ser nästan maläten ut men duger i lampskenet.",
                Bekladnadstyp.Plagg, Kroppsdel.Torso, BekladnadsLager.Mellan,
                false, true, true,
                "Du knäpper skjortan hela vägen upp i halsen. Lite trångt, men prydligt.",
                "Du knäpper upp och tar av dig skjortan."));

            SakerIRummet.Add(new Klader(
                "Finbyxor",
                "Ett par svarta pressveckade byxor.",
                Bekladnadstyp.Plagg, Kroppsdel.Nederdel, BekladnadsLager.Mellan,
                false, true, true,
                "Du drar på dig finbyxorna. Pressvecken är skarpa nog att skära smör med.",
                "Du kliver ur finbyxorna."));

            SakerIRummet.Add(new Klader(
        "Finstrumpor",
        "Ett par tunna, svarta strumpor. Helt utan hål vid tårna.",
        Bekladnadstyp.Plagg, Kroppsdel.Fot, BekladnadsLager.Underst,
        false, true, true,
        "Du drar på dig finstrumporna. De känns betydligt lyxigare än dina vanliga.",
        "Du tar av dig finstrumporna."));
        }

        public override void UndersokRum(Spelare spelare, StoryState story)
        {
            // 1. Kör baslogiken (visar rumsbeskrivningen)


            // 2. Logiken som faktiskt "öppnar" garderoben och tar fram kläderna
            var doldaSaker = SakerIRummet.Where(s => s.ArGomd).ToList();

            if (doldaSaker.Any())
            {
                WriteLine("\nDu öppnar garderoben. Där inne hänger dina finkläder, prydligt upphängda.");

                foreach (var sak in doldaSaker)
                {
                    sak.ArGomd = false; // Nu blir de synliga och går att 'ta'
                }
            }
        }
    }
}
