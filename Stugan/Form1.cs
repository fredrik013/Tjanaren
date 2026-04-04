using Stugan.Core;
using Stugan.Modeller.Saker;

namespace Stugan
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        ListBox lstInventory;
        NavButton btnNav;
        Spelmotor motor;

        public Form1()
        {
            InitializeComponent();
            motor = new Spelmotor();

            btnNav = new NavButton
            {
                Text = "Navigation (Här kan du gå med pilarna)",
                Dock = DockStyle.Bottom,
                Height = 50,
                TabIndex = 1
            };

            lstInventory = new ListBox
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Font = new Font("Consolas", 12),
                TabIndex = 2
            };

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

            // --- NAVIGATION ---
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

            // --- RYGGSÄCK (ÖPPEN) ---
            if (motor.IsInventoryOpen && lstInventory.Visible)
            {
                if (tryckt == ConsoleKey.Escape || tryckt == ConsoleKey.I)
                {
                    // Stäng logiskt i motorn
                    motor.HanteraInmatning(ConsoleKey.Escape);

                    // FIX: Vi använder HamtaStartText() som vi VET finns och 
                    // som returnerar rumsnamnet/beskrivningen.
                    string rumsNamn = motor.HamtaStartText();
                    VisaRum(rumsNamn);
                    return;
                }

                motor.Ryggsack.MarkeratIndex = lstInventory.SelectedIndex;
                string resultatInv = motor.HanteraInmatning(tryckt);

                if (!string.IsNullOrEmpty(resultatInv))
                {
                    rtbDisplay.Clear();
                    rtbDisplay.AppendText(resultatInv);
                    lstInventory.AccessibleName = resultatInv;
                }
                VisaRyggsack();
                return;
            }

            // --- KOMMANDON (I, S, U) ---
            if (tryckt == ConsoleKey.I || tryckt == ConsoleKey.S || tryckt == ConsoleKey.U)
            {
                string resultatKommando = motor.HanteraInmatning(tryckt);
                if (motor.IsInventoryOpen)
                {
                    rtbDisplay.Clear();
                    VisaRyggsack();
                }
                else
                {
                    VisaRum(resultatKommando);
                }
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

            rtbDisplay.Clear();

            if (!string.IsNullOrEmpty(resultat))
            {
                rtbDisplay.AppendText(resultat);
            }

            rtbDisplay.Focus();
        }
    }

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
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }
    }
}