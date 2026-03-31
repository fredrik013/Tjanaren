using NetCoreAudio;

public static class Ljudspelare
{
    private static Player _spelare = new Player();

    public static void Spela(string filnamn)
    {
        // Vi antar att ljudfilerna ligger i en mapp som heter "Ljud"
        string stig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ljud", filnamn);

        if (File.Exists(stig))
        {
            _spelare.Play(stig);
        }
    }
}