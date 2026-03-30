using Stugan.Core;
using Stugan.Modeller.Saker;
using static System.Console;

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
            if (s.AktivtSkodon == Bekladnadstyp.Ute.ToString())
            {
                WriteLine("\n[SYSTEMMEDDELANDE: MATTVÄKTARE 1.0 AKTIVERAD]");
                WriteLine("När du sätter foten över den fläckfria tröskeln sprakar TV:n till med ett ljud som påminner om glas som krossas.");
                WriteLine("Den raspiga rösten ekar nu direkt från bildröret, råare än i hallen:");

                WriteLine("\n'...MINA PERSISKA MATTOR ÄR INTE TILL FÖR DINA LERIGA SULOR!...'");
                WriteLine($"'...UT MED DIG! TA AV DIG DINA {s.AktivtSkodonNamn.ToUpper()} OCH KOM INTE TILLBAKA FÖRRÄN DU GÅR PÅ TÅ!...'");

                WriteLine("\nDet flimrande ljuset från skärmen tycks jaga skuggorna mot dina fötter.");
                WriteLine("Tryck på valfri tangent för att kliva in (på egen risk)...");
                ReadKey(true);
            }

            // Vi släpper in dem ändå så att de får ta konsekvensen (doften) inne i rummet
            return true;
        }

        public override void UndersokRum(Spelare spelare, Core.StoryState story)
        {
            var rum = spelare.NuvarandeRum;
            // Vi letar fortfarande efter TV-objektet för att veta om vi kan interagera
            var tv = rum.SakerIRummet.FirstOrDefault(s => s.Namn.Equals("TV", StringComparison.OrdinalIgnoreCase));

            if (luktarSvett)
            {
                WriteLine("\nDet vilar en tung, unken doft av fotsvett i rummet.");
            }


            if (tv != null)
            {
                WriteLine("\nDu går fram till den brusande TV:n.");
                WriteLine("Genom det gråa flimret hörs en raspig röst:");

                if (!story.HarBlivitVarnadOmRor) // Om spelaren latat sig!
                {
                    WriteLine("Skärmen lyser upp i ett giftigt lila sken och volymen maxas:");
                    WriteLine("\n'...SITT INTE HÄR OCH LATA DIG!...'");
                    WriteLine("'...NER I KÄLLAREN OCH LAGA DET TRASIGA RÖRET, ANNARS JÄVLAR!...'");
                    story.HarBlivitVarnadOmRor = true;
                    return; // Vi avbryter här så de inte får resten av tipsen förrän de jobbat lite!
                }
                else
                {
                    // HÄR ÄR DEN NYA RENA LOGIKEN (Ingen hårdkodning av namn!)
                    // HÄR ÄR DEN NYA RENA LOGIKEN (Nu helt synkad med din Enum!)
                    switch (spelare.AktivtSkodon)
                    {
                        case var s when s == Bekladnadstyp.Ute.ToString():
                            WriteLine("'...smutsen från utsidan följer dina steg... det gillas inte...'");
                            break;

                        case var s when s == Bekladnadstyp.Inne.ToString():
                            WriteLine("'...mjuka steg... ett klokt val... men de räcker inte till alla... sök i källaren...'");
                            break;

                        case var s when s == Bekladnadstyp.Skydd.ToString():
                            WriteLine("'...stövlar av gummi... redo för vätan... men akta så du inte dränker minnet...'");
                            break;

                        default:
                            WriteLine("'...barfota och blöt... jag känner doften av instängda boots ända hit...'");
                            break;
                    }

                    System.Threading.Thread.Sleep(500);
                    WriteLine("'...bzzzzt... prata med läskamraten... bzzzzt...'");
                }
            }
        }

        public override void ReageraPaHandling(string handling)
        {
            // Vi rensar bort ToLower här om vi vill vara strikta, 
            // men eftersom detta ofta kommer från användarkommandon kan det vara kvar.
            switch (handling.ToLower())
            {
                case "vadra":
                    if (luktarSvett)
                    {
                        luktarSvett = false;
                        WriteLine("\n[KORSDRAG]");
                        WriteLine("Frisk luft strömmar in från glasverandan och sveper med sig den unkna doften ut.");
                    }
                    break;

                // Vi använder interpolerade strängar för att matcha våra Enums exakt
                case string h when h == $"pa_{Bekladnadstyp.Ute.ToString().ToLower()}":
                    WriteLine("\n[TV:N SPRAKAR TILL]");
                    WriteLine("'...smutsen från utsidan följer dina steg... det gillas inte...'");
                    break;

                case string h when h == $"pa_{Bekladnadstyp.Inne.ToString().ToLower()}":
                    WriteLine("\n[TV:N FLIMRAR TILL]");
                    WriteLine("'...mjuka steg... ett klokt val... men sök vidare...'");
                    break;

                case string h when h == $"av_{Bekladnadstyp.Ute.ToString().ToLower()}":
                    luktarSvett = true;
                    WriteLine("\n[TV:N GER IFRÅN SIG ETT DISKRET PIP]");
                    WriteLine("'...äntligen... men aromen av instängda fötter dröjer sig kvar...'");
                    WriteLine("En lätt dimma av tveksam doft sprider sig från de varma bootsen.");
                    break;

                case string h when h == $"av_{Bekladnadstyp.Inne.ToString().ToLower()}":
                    WriteLine("\n[TV:N KNÄPPER TILL]");
                    WriteLine("Rummet känns plötsligt väldigt tyst när du tar av dig tofflorna.");
                    break;

                // Glöm inte skyddsskorna/gummistövlarna!
                case string h when h == $"pa_{Bekladnadstyp.Skydd.ToString().ToLower()}":
                    WriteLine("\n[TV:N BRUSAR UPPA]");
                    WriteLine("'...gummi mot golv... du förbereder dig för djupet...'");
                    break;
            }
        }
    }
}
