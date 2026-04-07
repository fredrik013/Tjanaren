using Forvaltaren.Core;
using Forvaltaren.Modeller;

namespace Forvaltaren.Modeller.Saker
{
    public enum Bekladnadstyp
    {
        Inne = 0,
        Ute = 1,
        Skydd = 2,
        Plagg = 3,
        Arbete = 4
    }

    public enum Kroppsdel
    {
        Huvud = 0,
        Torso = 1,
        Hand = 2,
        Fot = 3,
        Nederdel = 4,
        Helakroppen = 5
    }

    public enum BekladnadsLager
    {
        Underst = 0,
        Mellan = 1,
        Ytterst = 2
    }

    public class Klader : Spelsak
    {
        public Bekladnadstyp Typ { get; set; }

        public Kroppsdel Placering { get; set; }

        public BekladnadsLager Lager { get; set; }

        public string MeddelandePa { get; set; } // Ny: Eget meddelande vid PÅ

        public string MeddelandeAv { get; set; } // Ny: Eget meddelande vid AV

        public bool SkyddarMotVatten { get; set; }

        public Klader(string namn, string beskrivning, Bekladnadstyp typ, Kroppsdel placering, BekladnadsLager lager, bool skyddarmotvatten,
                              bool kanplockasupp, bool argomd, string meddelandePa = "", string meddelandeAv = "")
                    : base(namn, beskrivning, kanplockasupp, argomd)
        {
            Typ = typ;
            Placering = placering;
            Lager = lager;
            SkyddarMotVatten = skyddarmotvatten;
            MeddelandePa = string.IsNullOrEmpty(meddelandePa) ? $"Du tar på dig {namn}." : meddelandePa;
            MeddelandeAv = string.IsNullOrEmpty(meddelandeAv) ? $"Du tar av dig {namn}." : meddelandeAv;
        }

        public override string Anvand(Spelare spelare, StoryState story)
        {
            ArAktiv = !ArAktiv;
            string meddelande = ArAktiv ? MeddelandePa : MeddelandeAv;

            // Här ser vi till att oavsett om enumen heter Bekladnadstyp 
            // så skickar vi "pa_inne", "pa_ute" eller "pa_skydd" till rummet.
            string handlingStrang = (ArAktiv ? "pa_" : "av_") + Typ.ToString().ToLower();
            spelare.NuvarandeRum.ReageraPaHandling(handlingStrang);

            return meddelande;
        }
    }
}
