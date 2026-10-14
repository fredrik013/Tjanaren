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


        public override bool KanGaIn(Spelare s, StoryState story)
        {
            // Vi matchar mot Enum-namnen
            switch (s.AktivtSkodon)
            {
                case var skodon when skodon == Bekladnadstyp.Inne.ToString():
                    MessageBox.Show(
                        "[EN STRÄNG RÖST FRÅN VERANDAN]\n\n" +
                        "'Men vad sysslar du med?! Gå inte ut här och skita ner dina tofflor!'\n" +
                        "'Visa lite hyfs och byt om till något som tål lera innan du sätter din fot på gräsmattan.'",
                        "Tillträde förbjudet",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);

                    return false;

                case var skodon when skodon == Bekladnadstyp.Ute.ToString():
                    MessageBox.Show(
                        "Det klafsar till när bootsen möter den mjuka jorden.\n" +
                        "Du ser hur sulorna omedelbart täcks av lera.",
                        "Trädgården",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return true;

                case var skodon when skodon == Bekladnadstyp.Skydd.ToString():
                    MessageBox.Show(
                        "Gummistövlarna klafsar tryggt i leran. Det här är deras rätta element.",
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

    }
}
