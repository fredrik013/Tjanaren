using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Tradgard : Rum
    {
        public Tradgard() : base("Trädgården",
            "Du kliver ut i den vildvuxna trädgården. Gräset är högt och fuktigt, " +
            "och doften av våt jord är stark. Här och var ser man spår av gamla odlingar. Åt sydost skymtar man en grind mot gården.")
        {
            VisaNamn = "i trädgården";
            UtgangsMeddelanden.Add("Soder", "Du följer husväggen söderut och kommer in på en stig som leder fram till grinden.");
            OppnaUtgangar("Soder", "Norr");
            SakerIRummet.Add(new Redskap("Rörtång", "En tung och lite rostig rörtång. Perfekt för trilskande rör.", true, true));
        }

        private bool fixatRabatt = false;
        private bool klipptGras = false;

        public override string UndersokRum(Spelare spelare, StoryState story)
        {
            string output = "";
            var rortang = SakerIRummet.FirstOrDefault(s => s.Namn == "Rörtång");

            if (rortang != null && rortang.ArGomd)
            {
                // Vi kollar om spelaren har en AKTIV spade
                var aktivSpade = spelare.Ryggsack.GetAllaSaker()
                    .OfType<Redskap>()
                    .FirstOrDefault(r => r.Namn == "Spade" && r.ArAktiv);

                if (aktivSpade != null)
                {
                    rortang.ArGomd = false;
                    story.HarHittatRortang = true;
                    output += "\nDu greppar tag i spaden och sätter bladet i den mjuka jorden.\n";
                    output += "Klonk! Spaden träffar något hårt. Du drar upp en rostig rörtång!\n";
                    output += "\n" + ReageraPaHandling("gravt_i_rabatten");
                }
                else
                {
                    // Vi ger olika ledtrådar beroende på om spaden finns i säcken eller inte
                    bool harSpadeMenInaktiv = spelare.Ryggsack.GetAllaSaker().Any(s => s.Namn == "Spade");

                    if (harSpadeMenInaktiv)
                    {
                        output += "\nDet ser ut som att någon har grävt här tidigare. Markytan är lös.\n";
                        output += "Du provar att krafsa lite med fingrarna, men det är lönlöst.\n";
                        output += "Du behöver nog ta fram ett riktigt redskap om du ska komma någon vart.\n";
                    }
                    else
                    {
                        output += "\nDu rotar runt i gräset. Marken känns mjuk och uppbökad på ett ställe.\n";
                        output += "Det verkar finnas något här under, men du behöver nog ett verktyg för att få upp det.\n";
                    }
                }
            }
            else
            {
                output += "\nDu ser inget annat än blött gräs och lera här ute.\n";
            }

            return output;
        }


        public override bool KanGaIn(Spelare s, StoryState story, out string meddelande)
        {
            meddelande = "";
            // Vi matchar mot Enum-namnen
            switch (s.AktivtSkodon)
            {
                case var skodon when skodon == Bekladnadstyp.Inne.ToString():
                    MessageBox.Show(
                        "[EN STRÄNG RÖST FRÅN VERANDAN]\n\n" +
                        $"'Men vad sysslar du med?! Gå inte ut här och skita ner dina {s.AktivtSkodonNamn}!'\n" +
                        "'Visa lite hyfs och byt om till något som tål lera innan du sätter din fot på gräsmattan.'",
                        "Tillträde förbjudet",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);

                    return false;

                case var skodon when skodon == Bekladnadstyp.Ute.ToString():
                    MessageBox.Show(
                        $"Det klafsar till när dina {s.AktivtSkodonNamn} möter den mjuka jorden.\n" +
                        "Du ser hur sulorna omedelbart täcks av lera.",
                        "Trädgården",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;

                case var skodon when skodon == Bekladnadstyp.Skydd.ToString():
                    MessageBox.Show(
                        $"Dina {s.AktivtSkodonNamn} klafsar tryggt i leran. Det här är deras rätta element.",
                        "Trädgården",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;

                default:
                    MessageBox.Show(
                        "Du kliver ut barfota i det våta gräset. Det är kallt och klibbigt.",
                        "Trädgården",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;
            }
        }

        public override string ReageraPaHandling(string handling)
        {
            string output = "";

            switch (handling.ToLower())
            {
                case "gravt_i_rabatten":
                    output = "Plötsligt hörs herr von Krångels ursinniga röst från glasverandan!\n\n" +
                             "\"VAD I HELA HELVETE TAR DU DIG TILL MED?! " +
                             "DU UNDERSTÅR DIG VÄL INTE ATT GRÄVA I MIN FINA TRÄDGÅRD! " +
                             "NU ÄR DU SÅ GOD OCH ÅTERSTÄLLER DET DÄR, " +
                             "OCH NÄR DU ÄNDÅ ÄR IGÅNG KAN DU KLIPPA GRÄSET OCKSÅ!\"";
                    break;

                case "klipper_graset":
                    output = "Plötsligt hörs herr von Krångels irriterade röst från glasverandan!\n\n" +
                             "\"VAD I HELA FRIDENS NAMN ÄR DET FÖR ETT FÖRSKRÄCKLIGT OVÄSEN?! " +
                             "MÅSTE DU FÖRA ETT SÅDANT HERRANS LIV NÄR DU ARBETAR?!\"\n\n" +
                             "Du låtsas inte höra honom utan fortsätter envist att kämpa med den gnisslande handjagaren.";
                    break;
            }

            return output;
        }

        public override string UtforUnderhall(Spelare spelare, StoryState story)
        {
            if (story.HarFixatTradgard)
            {
                return "Trädgården är redan iordningställd. " +
                       "Här finns inget mer att göra.";
            }

            string output = "";

            var redskap = spelare.Ryggsack.GetAllaSaker()
                .OfType<Redskap>();

            bool harSpade = redskap.Any(r =>
                r.Namn == "Spade" && r.ArAktiv);

            bool harGrasklippare = redskap.Any(r =>
                r.Namn == "Gräsklippare" && r.ArAktiv);

            switch (harSpade, harGrasklippare)
            {
                case (false, false):
                    output += "Du ser dig omkring i trädgården. " +
                              "Rabatten behöver återställas och " +
                              "gräsmattan klippas, men du har inga " +
                              "lämpliga redskap framtagna.\n";
                    break;

                case (true, false):
                    if (!fixatRabatt)
                    {
                        output += "Med största motvilja börjar du " +
                                  "återställa rabatten. Du skyfflar " +
                                  "tillbaka jorden med spaden och " +
                                  "försöker få allt att se ut som tidigare.\n\n" +
                                  "När du äntligen är färdig betraktar " +
                                  "du resultatet. Kanske är det bäst " +
                                  "att vattna också, tänker du.\n\n" +
                                  "Du får syn på en vattenkanna bredvid " +
                                  "rabatten och ger den återställda " +
                                  "jorden en ordentlig skvätt vatten.\n";

                        fixatRabatt = true;
                    }
                    else
                    {
                        output += "Rabatten är redan återställd.\n";
                    }

                    if (!klipptGras)
                    {
                        output += "\nGräsmattan behöver fortfarande klippas.\n";
                    }
                    break;

                case (false, true):
                    if (!klipptGras)
                    {
                        output += "Du börjar klippa gräset med den " +
                                  "gamla handjagaren som gnisslar och " +
                                  "väsnas så det ekar över hela kvarteret. " +
                                  "Du pustar och stånkar över åbäket.\n\n";

                        output += ReageraPaHandling("klipper_graset");

                        output += "\n\nEfter ett rejält slit är du " +
                                  "äntligen färdig.\n\n" +
                                  "När du betraktar resultatet upptäcker " +
                                  "du att gräset längs kanterna fortfarande " +
                                  "är för långt. Du hittar en gammal sax " +
                                  "och sätter dig på knä för att omsorgsfullt " +
                                  "klippa de sista stråna.\n\n" +
                                  "Till slut reser du dig med värkande rygg " +
                                  "och betraktar ditt arbete. Inte ens " +
                                  "herr von Krångel borde kunna hitta " +
                                  "något att klaga på.\n\n" +
                                  "Men man vet förstås aldrig.\n";

                        klipptGras = true;
                    }
                    else
                    {
                        output += "Gräsmattan är redan klippt.\n";
                    }

                    if (!fixatRabatt)
                    {
                        output += "\nRabatten behöver fortfarande återställas.\n";
                    }
                    break;

                case (true, true):
                    if (!fixatRabatt)
                    {
                        output += "Med största motvilja återställer du " +
                                  "rabatten med spaden. Du skyfflar " +
                                  "tillbaka jorden och försöker få allt " +
                                  "att se ut som tidigare.\n\n" +
                                  "När du är färdig upptäcker du en " +
                                  "vattenkanna bredvid rabatten och " +
                                  "ger jorden en ordentlig skvätt vatten.\n\n";

                        fixatRabatt = true;
                    }

                    if (!klipptGras)
                    {
                        output += "Du börjar klippa gräset med den " +
                                  "gamla handjagaren. Den gnisslar och " +
                                  "väsnas så det ekar över hela kvarteret. " +
                                  "Du pustar och stånkar över åbäket.\n\n";

                        output += ReageraPaHandling("klipper_graset");

                        output += "\n\nDu låter inte von Krångels " +
                                  "klagomål hindra arbetet. Efter ett " +
                                  "rejält slit är gräsmattan klippt.\n\n" +
                                  "Du upptäcker att gräset längs kanterna " +
                                  "fortfarande är för långt och hittar " +
                                  "en gammal sax. På knä klipper du " +
                                  "omsorgsfullt de sista stråna.\n\n" +
                                  "Till slut reser du dig med värkande " +
                                  "rygg. Inte ens herr von Krångel " +
                                  "borde kunna klaga på resultatet.\n\n" +
                                  "Men man vet förstås aldrig.\n";

                        klipptGras = true;
                    }
                    break;
            }

            if (fixatRabatt && klipptGras)
            {
                story.HarFixatTradgard = true;
                output += "\nÄntligen! Både rabatten och gräsmattan " +
                          "är iordningställda. Trädgårdsarbetet är klart!\n";
            }

            return output;
        }

    }
}
