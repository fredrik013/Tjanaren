using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        ListBox lstInventory;
        NavButton btnNav; // Ändrad till vår nya klass
        Spelmotor motor;

        public Form1()
        {
            InitializeComponent();
            motor = new Spelmotor();

            // 1. Skapa vår special-knapp som tillåter pilar
            btnNav = new NavButton
            {
                Text = "Navigation (Här kan du gå med pilarna)",
                Dock = DockStyle.Bottom,
                Height = 50,
                TabIndex = 1
            };

            // 2. Skapa Inventory-listan
            lstInventory = new ListBox
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Font = new Font("Consolas", 12),
                TabIndex = 2
            };

            // 3. Skapa Displayen
            rtbDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12),
                TabIndex = 3
            };

            this.Controls.Add(lstInventory);
            this.Controls.Add(rtbDisplay);
            this.Controls.Add(btnNav);

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            string startHalsning = motor.HamtaStartText();
            VisaRum(startHalsning);

            rtbDisplay.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ConsoleKey tryckt = (ConsoleKey)e.KeyCode;

            // --- NAVIGATION (Nu lyssnar den faktiskt!) ---
            if (btnNav.Focused)
            {
                if (tryckt == ConsoleKey.UpArrow || tryckt == ConsoleKey.DownArrow ||
                    tryckt == ConsoleKey.LeftArrow || tryckt == ConsoleKey.RightArrow)
                {
                    string resultat = motor.HanteraInmatning(tryckt);
                    VisaRum(resultat);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            // --- RYGGSÄCK ---
            if (motor.IsInventoryOpen && lstInventory.Visible)
            {
                motor.Ryggsack.MarkeratIndex = lstInventory.SelectedIndex;
                string resultat = motor.HanteraInmatning(tryckt);

                if (!string.IsNullOrEmpty(resultat))
                {
                    rtbDisplay.Clear();
                    rtbDisplay.AppendText(resultat);
                    lstInventory.AccessibleName = resultat;
                }
                VisaRyggsack();
                lstInventory.AccessibleName = null;
                return;
            }

            // --- I, S, U ---
            if (tryckt == ConsoleKey.I || tryckt == ConsoleKey.S || tryckt == ConsoleKey.U)
            {
                string resultat = motor.HanteraInmatning(tryckt);
                if (motor.IsInventoryOpen) VisaRyggsack();
                else VisaRum(resultat);
            }
        }

        private void VisaRyggsack()
        {
            btnNav.Visible = false;
            rtbDisplay.Visible = false;
            lstInventory.Visible = true;

            int nuvarandeIndex = lstInventory.SelectedIndex;
            lstInventory.Items.Clear();
            foreach (var sak in motor.Ryggsack.GetAllaSaker())
            {
                string status = "";
                if (sak is Klader k) status = k.ArAktiv ? " (påtagen)" : " (i väskan)";
                else if (sak is Redskap r) status = r.ArAktiv ? " (i handen)" : " (i väskan)";
                else if (sak is AllmanSak a) status = a.ArAktiv ? " (i handen)" : " (i väskan)";
                else if (sak is Livsmedel l) status = l.ArAktiv ? " (i handen)" : " (i väskan)";
                lstInventory.Items.Add($"{sak.Namn}{status}");
            }

            if (lstInventory.Items.Count > 0)
                lstInventory.SelectedIndex = nuvarandeIndex != -1 ? nuvarandeIndex : 0;

            lstInventory.Focus();
        }

        private void VisaRum(string resultat)
        {
            lstInventory.Visible = false;
            btnNav.Visible = true;
            rtbDisplay.Visible = true;

            if (!string.IsNullOrEmpty(resultat))
            {
                rtbDisplay.Clear();
                rtbDisplay.AppendText(resultat);
            }
        }
    }

    // DENNA KLASS LÖSER PROBLEMET:
    // En vanlig knapp sväljer piltangenter. Denna version säger till Windows att
    // vi vill använda pilarna själva.
    public class NavButton : Button
    {
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true; // Ja, jag vill ha dessa tangenter!
                default:
                    return base.IsInputKey(keyData);
            }
        }
    }
}