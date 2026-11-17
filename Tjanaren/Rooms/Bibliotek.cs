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

            Random rnd = new Random();
            var samtalsOrdning = Enumerable.Range(0, 8)
                .OrderBy(x => rnd.Next())
                .Take(8)
                .ToList();
            foreach (int samtal in samtalsOrdning)
            {

                switch (samtal)
            {
                case 0 when story.UtfordaUppgifter == 0:                   
                        meddelande += "”Jag vet inte vad jag har anställt " +
                                      "för folk! Tjänaren har varit anställd " +
                                      "i flera dagar och inte presterat ett " +
                                      "jävla dugg!”";
                    break;

                case 1 when story.HarFixatTradgard == true:
                    Tjuvlyssnat++;
                        if (Tjuvlyssnat == 3)
                        {
                            TelefonLast = true;
                            story.AntalOvertramp++;

                            return meddelande += "\n\nPlötsligt hör du steg bakom dig. " +
                                "Herr von Krångel har smugit sig fram " +
                                "med sin trådlösa telefon och ertappar dig!\n\n" +
                                "”JAG HOPPAS VERKLIGEN ATT DU HAR EN BRA FÖRKLARING! " +
                                "DET HÄR ÄR ETT ALLVARLIGT ÖVERTRAMP! " +
                                "Jag tänker anteckna det här i protokollet. " +
                                "Du får en prick för ditt tilltag, och jag förväntar mig " +
                                "att det inte upprepas!”";
                        }
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

                    case 2 when story.HarDruckitSprit == true:
                    meddelande += "”Jag tror att jag har anställt en alkoholist. Här om dagen tog jag tjänaren på bar gärning med att dricka av min finaste whisky!”";
                    break;

                case 3 when story.HallenArSkurad == false:
                    meddelande += "”Tjänarna tror tydligen, trots upprepade tillsägelser, att de kan klampa runt hur som helst med sina skitiga skor! Den jag har anställt nu klampade rakt in på den fina mattan i inre hallen, men inte ens efter det har han bemödat sig med att städa upp efter sig!”";
                    break;

                    case 4 when story.StinkandeHall == true:
                    meddelande += "”Nu har tjänaren visat att han har riktigt dåligt omdöme. Häromdagen kände jag att övre hallen stank som värsta ostfabriken! När jag kom upp upptäckte jag att tjänaren hade den dåliga smaken att knalla omkring där barfota och fläcka ner min fina matta!”";
                    break;

                    case 5 when story.HarAnvantSpabad && story.TagitRakhyvel == true:
                    meddelande += "”Jag vet inte vad jag ska ta mig till med den här tjänaren. Det verkar inte finnas några gränser för vad han tar sig till! Häromdagen ertappade jag honom med att ligga och slappa i mitt spabad. Och som om inte det vore nog hade han mage att ta min exklusiva rakhyvel och använda den också!”";
                    break;

                    case 6 when story.HarAnvantSpabad == true:
                    meddelande += "”Häromdagen ertappade jag tjänaren med att ligga och slappa i mitt fina spabad. Med tanke på hur han sköter sin hygien vill jag inte ens tänka på vilket jobb jag får med att sanera det! Jag hotade med att låta honom följa med vattnet ner i avloppet!” " + "”Men min vän, hur tänker du egentligen? Hur ska tjänaren kunna duscha och sköta sin hygien om han inte ens får använda spabadet?” " + "”Det finns faktiskt en dusch i källaren som han kan använda!”" + "”Den där gamla sunkiga duschen? Jag förstår om du inte får behålla din personal!”";
                    break;

                    case 7 when story.TagitRakhyvel == true:
                    meddelande += "”Jag börjar verkligen undra vad det är för en tjänare jag har anställt! Häromdagen upptäckte jag att han hade tagit min exklusiva rakhyvel och använt den! Man kan tydligen inte ens lämna sina personliga tillhörigheter obevakade i det här huset!”";
                    break;

                    default:
                        continue;
            }

            Tjuvlyssnat++;

                if (Tjuvlyssnat == 3)
                {
                    TelefonLast = true;
                    story.AntalOvertramp++;

                    return meddelande += "\n\nPlötsligt hör du steg bakom dig. " +
                        "Herr von Krångel har smugit sig fram " +
                        "med sin trådlösa telefon och ertappar dig!\n\n" +
                        "”JAG HOPPAS VERKLIGEN ATT DU HAR EN BRA FÖRKLARING! " +
                        "DET HÄR ÄR ETT ALLVARLIGT ÖVERTRAMP! " +
                        "Jag tänker anteckna det här i protokollet. " +
                        "Du får en prick för ditt tilltag, och jag förväntar mig " +
                        "att det inte upprepas!”";
                }
                return meddelande;
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
