using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Bibliotek : Rum
    {
        private int Tjuvlyssnat = 0;
        private bool TelefonLast = false;

        public Bibliotek() : base("Biblioteket", "Du har kommit in i husets pampiga bibliotek. Väggarna täcks av bokhyllor från golv till tak. I mitten finns ett bord där man kan sitta och läsa. Mitt på bordet står en telefon som fångar din uppmärksamhet.")
        {
            VisaNamn = "i biblioteket";
            OppnaUtgangar("Vaster");
            SakerIRummet.Add(new Inredning("Telefon", "En telefon av äldre modell. Du känner dig frestad att lyfta på luren och se om den fungerar."));
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            string meddelande = "\nDu lyfter på luren och i stället för " +
                    "en kopplingston hör du till din förvåning " +
                    "herr von Krångels röst. Han verkar sitta " +
                    "och prata med någon i telefon.\n\n";
            if (TelefonLast == true)
                return  meddelande += "Du har redan fått en prick i protokollet. Det är nog inte värt att tjuvlyssna fler gånger.";

            switch (Tjuvlyssnat)
            {
                case 0 when story.UtfordaUppgifter == 0:                   
                        meddelande += "”Jag vet inte vad jag har anställt " +
                                      "för folk! Tjänaren har varit anställd " +
                                      "i flera dagar och inte presterat ett " +
                                      "jävla dugg!”";
                    break;

                case 1 when story.HarFixatTradgard == true:
                    Tjuvlyssnat++;
                    return meddelande +=
                        "”Du skulle ha hört vilket förskräckligt oväsen " +
                        "det var från trädgården när tjänaren klippte gräset!”\n\n" +
                        "”Jaså, har du fortfarande kvar den gamla handjagaren? " +
                        "Du borde väl ha bytt ut den för länge sedan.”\n\n" +
                        "”EN NY?! Den gamla fungerar alldeles utmärkt!”\n\n" +
                        "”Det är kanske inte tjänaren det är fel på?”\n\n" +
                        "”JAG RINGDE INTE FÖR ATT FÅ EN FÖRELÄSNING OM " +
                        "MIN GRÄSKLIPPARE!”\n\n" +
                        "Du kokar av ilska men till slut utbrister du:\n\n" +
                        "”DIN JÄVLA SNÅLJÅP!\n\n”" + "Det blir dödstyst i luren.\n\n" +
                                 "HALLÅ?! VEM ÄR DET DÄR?! " +
                                 "ÄR DET DU?! STÅR DU OCH TJUVLYSSNAR PÅ " +
                                 "MINA PRIVATA TELEFONSAMTAL?! HAR DU INGET " +
                                 "ARBETE ATT UTFÖRA?!";
            }

            Tjuvlyssnat++;

            if (Tjuvlyssnat == 3)
            {
                TelefonLast = true;
                story.AntalOvertramp++;
                return meddelande += "\n\nPlötsligt hör du steg bakom dig. " +
                  "Herr von Krångel har smugit sig fram " +
                  "med sin trådlösa telefon och ertappar dig!";
            }
            return meddelande += "\n\nDu försöker hålla tillbaka, men plötsligt " +
                                 "undslipper dig en kraftig hostning.\n\n" +
                                 "Det blir dödstyst i luren.\n\n" +
                                 "HALLÅ?! VEM ÄR DET DÄR?! " +
                                 "ÄR DET DU?! STÅR DU OCH TJUVLYSSNAR PÅ " +
                                 "MINA PRIVATA TELEFONSAMTAL?! HAR DU INGET " +
                                 "ARBETE ATT UTFÖRA?!";
        }
    }
}
