namespace Tjanaren.Core
{
    public class StoryState
    {
        // Uppgifter och framsteg i spelet

        public bool RorArLagat { get; set; } = false;

        public bool HeltRentIKallaren { get; set; } = false;

        public bool HallenArSkurad { get; set; } = false;

        public bool HarFixatTradgard { get; set; } = false;

        public int UtfordaUppgifter { get; set; } = 0;

        // Statusflaggor

        public bool HarHittatRortang { get; set; } = false;

        public bool TvattmaskinArIgang { get; set; }

        public bool SkurvattenKlart { get; set; } = false;

        public bool HarNyckel { get; set; } = false;

        // Handlingar och framsteg

        public int AntalOvertramp { get; set; } = 0; // 3 = Hejdå!

        public bool ArNyduschad { get; set; } = false;

        public bool HarAnvantSpabad { get; set; } = false;

        public bool HarBlivitVarnadOmRor { get; set; } = false;

        public bool HarBlivitUtskalldHallen { get; set; } = false;

        // Övriga flaggor

        public bool HarAtitBrod { get; set; } = false;

        public bool HarDruckitSprit { get; set; } = false;
    }
}
