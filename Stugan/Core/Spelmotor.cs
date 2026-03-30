namespace Stugan.Core
{
    public class Spelmotor
    {
        public string HamtaStartText()
        {
            return "Du står utanför stugan. Regnet piskar mot ansiktet.";
        }

        public string HamtaInventoryTest()
        {
            return "I din ryggsäck hittar du: Ett par boots.";
        }

        public string HanteraInmatning(ConsoleKey tangent)
        {
            // Här landar din gamla switch-logik sen
            switch (tangent)
            {
                case ConsoleKey.I:
                    return "Du rotar i ryggsäcken och hittar dina boots.";

                case ConsoleKey.L:
                    return "Rummet är ljust och du ser en gammal kamin i hörnet.";

                default:
                    return $"Du tryckte på {tangent}. Inget händer här än.";
            }
        }
    }
}