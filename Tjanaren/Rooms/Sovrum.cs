using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tjanaren.Modeller.Saker;

namespace Tjanaren.Rooms
{
    public class Sovrum : Rum
    {
        public Sovrum() : base("Sovrummet", "Du har kommit in i von Krångels privata sovrum. Sovrummet är exklusivt möblerat med inredning som ser dyrbar ut. Du lägger märke till den dyra sängen med det exklusiva nattduksbordet bredvid.")
        {
            VisaNamn = "i sovrummet";
            OppnaUtgangar("Oster");

SakerIRummet.Add(new Klader("Sidenkalsonger", "Det här ser ut att vara ett par riktigt exklusiva sidenkalsonger som antagligen kostar mer än du tjänar hos von Krångel.", Bekladnadstyp.Plagg, Kroppsdel.Nederdel, BekladnadsLager.Underst, false, true, false, "Du tar på dig kalsongerna.", "Du tar av dig kalsongerna."));
        }
    }
}
