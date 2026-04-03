using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        ListBox lstInventory; // Vår nya ListBox för ryggsäcken
        Spelmotor motor;

        public Form1()
        {
            InitializeComponent();

            motor = new Spelmotor();

            // 1. Skapa ListBoxen (gömd som standard)
            lstInventory = new ListBox
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Font = new Font("Consolas", 12)
            };

            // 2. Skapa RichTextBoxen
            rtbDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12)
            };

            // VIKTIGT: Lägg till båda kontrollerna i formuläret
            this.Controls.Add(lstInventory);
            this.Controls.Add(rtbDisplay);

            string startHalsning = motor.HamtaStartText();
            rtbDisplay.AppendText(startHalsning + "\n");

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            rtbDisplay.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ConsoleKey tryckt = (ConsoleKey)e.KeyCode;

            if (motor.IsInventoryOpen && lstInventory.Visible)
            {
                motor.Ryggsack.MarkeratIndex = lstInventory.SelectedIndex;
                string resultat = motor.HanteraInmatning(tryckt);

                if (!string.IsNullOrEmpty(resultat))
                {
                    // 1. Uppdatera textrutan i bakgrunden
                    rtbDisplay.Clear();
                    rtbDisplay.AppendText(resultat);

                    // 2. Tvinga JAWS att läsa upp resultatet utan att flytta fokus
                    // Vi sätter resultatet som listboxens namn tillfälligt, 
                    // vilket får skärmläsaren att reagera.
                    lstInventory.AccessibleName = resultat;

                    // Vi "puffar" på kontrollen så JAWS märker ändringen
                    lstInventory.Focus();
                }

                VisaRyggsack();

                // Återställ AccessibleName så den inte läser samma sak nästa gång du flyttar pilen
                lstInventory.AccessibleName = null;
            }
            else
            {
                string resultat = motor.HanteraInmatning(tryckt);
                if (motor.IsInventoryOpen) VisaRyggsack();
                else VisaRum(resultat);
            }
        }

        private void VisaRyggsack()
        {
            rtbDisplay.Visible = false;
            lstInventory.Visible = true;

            // Spara nuvarande markering så den inte hoppar till toppen vid varje tryck
            int nuvarandeIndex = lstInventory.SelectedIndex;

            lstInventory.Items.Clear();
            foreach (var sak in motor.Ryggsack.GetAllaSaker())
            {
                string status = "";
                if (sak is Klader k) status = k.ArAktiv ? " (påtagen)" : " (i ryggsäcken)";
                else if (sak is Redskap r) status = r.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is AllmanSak a) status = a.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is Livsmedel l) status = l.ArAktiv ? " (i handen)" : " (i ryggsäcken)";

                lstInventory.Items.Add($"{sak.Namn}{status}");
            }

            // Återställ markeringen (eller sätt till 0 om det är första gången)
            if (lstInventory.Items.Count > 0)
            {
                lstInventory.SelectedIndex = nuvarandeIndex != -1 ? nuvarandeIndex : 0;
            }

            lstInventory.Focus(); // Ger fokus till listan så JAWS börjar läsa
        }

        private void VisaRum(string resultat)
        {
            lstInventory.Visible = false;
            rtbDisplay.Visible = true;

            if (!string.IsNullOrEmpty(resultat))
            {
                rtbDisplay.Clear();
                rtbDisplay.AppendText(resultat);
            }

            rtbDisplay.Focus();
        }
    }
}