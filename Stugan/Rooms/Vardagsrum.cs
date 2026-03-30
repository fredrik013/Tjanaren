using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan.Rooms
{
    public class Vardagsrum : Rum
    {
        private bool luktarSvett = false;

        public Vardagsrum() : base("Vardagsrummet", "Du kliver in i vardagsrummet. En stor, mjuk soffa står framför en gammal tjock-TV som står och brusar. " +
        "Ljuset från skärmen fladdrar mot de mörka tapeterna.")
        {
            VisaNamn = "i vardagsrummet";
            OppnaUtgangar("Vaster", "Oster");
            SakerIRummet.Add(new Inredning("TV", "En gammal Philips-TV. Den visar bara myrornas krig, men ljudet är öronbedövande.", false, false));

            SakerIRummet.Add(new Inredning("Soffa", "En mjuk och pösig soffa med tjocka dynor. Undrar om den gömmer på några hemligheter."));
        }

        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi behåller logiken att man faktiskt får gå in (return true),
            // men vi tar bort konsol-interaktionen och väntandet.

            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                // Här skulle vi i framtiden kunna trigga en flagga i storyn
                // som gör att rummet "minns" att du smutsat ner mattan.
            }

            // Vi släpper in dem så att de får ta konsekvenserna inne i rummet
            return true;
        }

        public override string UndersokRum(Spelare spelare, Core.StoryState story)
        {
            string output = "";
            var rum = spelare.NuvarandeRum;
            var tv = SakerIRummet.FirstOrDefault(s => s.Namn.Equals("TV", StringComparison.OrdinalIgnoreCase));

            // Vi kollar doften (här antar jag att luktarSvett är en variabel i rummet eller storyn)
            if (luktarSvett)
            {
                output += "\nDet vilar en tung, unken doft av fotsvett i rummet.\n";
            }

            if (tv != null)
            {
                output += "\nDu går fram till den brusande TV:n.\n";
                output += "Genom det gråa flimret hörs en raspig röst:\n";

                if (!story.HarBlivitVarnadOmRor) // Om spelaren latat sig!
                {
                    output += "Skärmen lyser upp i ett giftigt lila sken och volymen maxas:\n";
                    output += "\n'...SITT INTE HÄR OCH LATA DIG!...'\n";
                    output += "'...NER I KÄLLAREN OCH LAGA DET TRASIGA RÖRET, ANNARS JÄVLAR!...'\n";
                    story.HarBlivitVarnadOmRor = true;
                    return output; // Vi avbryter här precis som i din originalkod
                }
                else
                {
                    // Synkat med din Enum precis som du ville!
                    switch (spelare.AktivtSkodon)
                    {
                        case var s when s == Bekladnadstyp.Ute.ToString():
                            output += "'...smutsen från utsidan följer dina steg... det gillas inte...'\n";
                            break;

                        case var s when s == Bekladnadstyp.Inne.ToString():
                            output += "'...mjuka steg... ett klokt val... men de räcker inte till alla... sök i källaren...'\n";
                            break;

                        case var s when s == Bekladnadstyp.Skydd.ToString():
                            output += "'...stövlar av gummi... redo för vätan... men akta så du inte dränker minnet...'\n";
                            break;

                        default:
                            output += "'...barfota och blöt... jag känner doften av instängda boots ända hit...'\n";
                            break;
                    }

                    // Ingen Thread.Sleep här, vi bara lägger till texten direkt
                    output += "\n'...bzzzzt... prata med läskamraten... bzzzzt...'\n";
                }
            }

            return output;
        }

        public override string ReageraPaHandling(string handling)
        {
            string output = "";

            // Vi kör ToLower för att matcha användarens kommandon smidigt
            switch (handling.ToLower())
            {
                case "vadra":
                    if (luktarSvett)
                    {
                        luktarSvett = false;
                        output += "\n[KORSDRAG]\n";
                        output += "Frisk luft strömmar in från glasverandan och sveper med sig den unkna doften ut.\n";
                    }
                    break;

                // Vi matchar mot Enums, precis som du byggt det
                case string h when h == $"pa_{Bekladnadstyp.Ute.ToString().ToLower()}":
                    output += "\n[TV:N SPRAKAR TILL]\n";
                    output += "'...smutsen från utsidan följer dina steg... det gillas inte...'\n";
                    break;

                case string h when h == $"pa_{Bekladnadstyp.Inne.ToString().ToLower()}":
                    output += "\n[TV:N FLIMRAR TILL]\n";
                    output += "'...mjuka steg... ett klokt val... men sök vidare...'\n";
                    break;

                case string h when h == $"av_{Bekladnadstyp.Ute.ToString().ToLower()}":
                    luktarSvett = true;
                    output += "\n[TV:N GER IFRÅN SIG ETT DISKRET PIP]\n";
                    output += "'...äntligen... men aromen av instängda fötter dröjer sig kvar...'\n";
                    output += "En lätt dimma av tveksam doft sprider sig från de varma bootsen.\n";
                    break;

                case string h when h == $"av_{Bekladnadstyp.Inne.ToString().ToLower()}":
                    output += "\n[TV:N KNÄPPER TILL]\n";
                    output += "Rummet känns plötsligt väldigt tyst när du tar av dig tofflorna.\n";
                    break;

                case string h when h == $"pa_{Bekladnadstyp.Skydd.ToString().ToLower()}":
                    output += "\n[TV:N BRUSAR UPPA]\n";
                    output += "'...gummi mot golv... du förbereder dig för djupet...'\n";
                    break;
            }

            return output;
        }
    }
}
