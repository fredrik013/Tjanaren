namespace Forvaltaren.Core
{
    public class StoryState
    {
        public bool HarHittatRortang { get; set; } = false;

        public bool RorArLagat { get; set; } = false;

        public bool HarBlivitVarnadOmRor { get; set; } = false;

        public bool GrovstadatPannrum { get; set; } = false;

        public bool HeltRentIKallaren { get; set; } = false;

        public bool MoppArTvattad { get; set; } = false;

        public bool TvattmaskinArIgang { get; set; }

        public bool HarBlivitUtskalldHallen { get; set; } = false;

        public bool HarAtitBrod { get; set; } = false;

        public bool HarDruckitSprit { get; set; } = false;

        public bool HarSmutsatNerMattan { get; set; } = false;

        public bool HallenArSkurad { get; set; } = false;

        public bool HarArbetskladerPaSig { get; set; } = false;

        public bool HarNyckel { get; set; } = false;

        public int AntalOvertramp { get; set; } = 0; // 3 = Hejdå!

        public bool GrasetKlippt { get; set; } = false;

        public bool ArNyduschad { get; set; } = false;

        public bool HarPlastratOmFinger { get; set; } = false;

        public bool HarSkuritSigVidRakning { get; set; } = false;

        public bool HarAnvantSpabad { get; set; } = false;

        public string GetStatusBeskrivning()
        {
            // Vi switchar på 'true' för att kunna utvärdera dina flaggor fritt i varje case
            switch (true)
            {
                // Prioritet 0: Du har åkt ut!
                case var _ when AntalOvertramp >= 3:
                    return "Du står på uppfarten med din väska. Finfolket har låst dörren. Det är slut.";

                case var _ when AntalOvertramp == 2:
                    return $"Du vinglar betänkligt och sjunger snapsvisor. Herr von Krångel håller hårt i dörrhandtaget. (Varningar: {AntalOvertramp}/3)";

                case var _ when AntalOvertramp == 1:
                    return $"Du doftar misstänkt mycket malt. Finfolket ser skeptiska ut, men du har en chans kvar. (Varningar: {AntalOvertramp}/3)";

                // Prioritet 1: Allt är klart
                case var _ when RorArLagat && HeltRentIKallaren && GrasetKlippt && ArNyduschad:
                    return "Du doftar lavendel och röret är tyst. Glasverandan väntar.";

                // Prioritet 2: Specifika hån (t.ex. brödtjuven)
                case var _ when HarAtitBrod && !RorArLagat:
                    return "Du ser misstänkt mätt ut för att inte ha rört ett finger i källaren än!";

                case var _ when HarSkuritSigVidRakning:
                    return "Du blöder ymnigt från hakan. Herr von Krångel ser inte imponerad ut över din hantering av hans hyvel.";

                case var _ when HarPlastratOmFinger && !ArNyduschad:
                    return "Fingret är omplåstrat, men du luktar fortfarande som en dränkt källarråtta. Dags för en dusch?";

                // Prioritet 3: Delmål i källaren
                case var _ when RorArLagat && !GrovstadatPannrum:
                    return "Läckan är stoppad, men det ser ut som en krigszon. Fram med skyffeln!";

                case var _ when RorArLagat:
                    return "Röret är lagat, men du har lämnat en rökig röra efter dig.";

                case var _ when HarHittatRortang:
                    return "Du har rörtången i säkert förvar. Nu är det väl bara att hitta pannrummet och börja jobba?";

                // Standard: Om ingenting har hänt än
                default:
                    return "Du har knappt börjat lyfta ett finger än.";
            }
        }
    }
}
