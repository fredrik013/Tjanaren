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

            switch (Tjuvlyssnat)
            {
                case 0:
                    if (story.UtfordaUppgifter == 0)
                    {
                        meddelande += "”Jag vet inte vad jag har anställt " +
                                      "för folk! Tjänaren har varit anställd " +
                                      "i flera dagar och inte presterat ett " +
                                      "jävla dugg!”";
                    }
                    break;
            }

            return meddelande;
        }
    }
}
