namespace Forvaltaren.Core
{
    /// <summary>
    /// Representerar en koordinat i spelvärlden (X, Y, Z).
    /// Använder 'record' för automatisk hantering av jämförelser.
    /// </summary>
    public record Position(int X, int Y, int Z);
}