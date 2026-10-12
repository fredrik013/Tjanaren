using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        ListBox lstInventory;
        ListBox lstRoomItems;
        NavButton btnNav;
        Spelare spelare;
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

            lstRoomItems = new ListBox
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Font = new Font("Consolas", 12),
                TabIndex = 4 // Nytt TabIndex
            };

            rtbDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12),
                TabIndex = 3
            };

            this.Controls.Add(lstInventory);
            this.Controls.Add(lstRoomItems);
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

            if (e.KeyCode == Keys.F6 || e.KeyCode == Keys.Tab)
            {
                bool framat = !e.Shift;

                this.SelectNextControl(this.ActiveControl, framat, true, true, true);

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            ConsoleKey tryckt = (ConsoleKey)e.KeyCode;

            // --- FÖREMÅLSVAL (NYTT) ---
            if (motor.IsSelectingItem && lstInventory.Visible)
            {
                if (tryckt == ConsoleKey.UpArrow || tryckt == ConsoleKey.DownArrow)
                {
                    motor.HanteraInmatning(tryckt);
                    VisaForemalsvalet();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }

                if (tryckt == ConsoleKey.Enter)
                {
                    string resultat = motor.HanteraInmatning(tryckt);

                    if (!motor.IsSelectingItem)
                    {
                        VisaRum(resultat);
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }

                if (tryckt == ConsoleKey.Escape)
                {
                    string resultat = motor.HanteraInmatning(tryckt);
                    VisaRum(resultat);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            // --- NAVIGATION ---
            if (btnNav.Focused)
            {
                if (tryckt == ConsoleKey.UpArrow || tryckt == ConsoleKey.DownArrow ||
                    tryckt == ConsoleKey.LeftArrow || tryckt == ConsoleKey.RightArrow || tryckt == ConsoleKey.PageDown || tryckt == ConsoleKey.PageUp)
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
                    motor.HanteraInmatning(ConsoleKey.Escape);

                    var rum = motor._spelare.NuvarandeRum;
                    string rumsInfo = $"{rum.Namn}\n{rum.Beskrivning}";
                    VisaRum(rumsInfo);
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

            // --- KOMMANDON (I, S, U, T, B) ---
            if (tryckt == ConsoleKey.I || tryckt == ConsoleKey.S || tryckt == ConsoleKey.U || tryckt == ConsoleKey.T || tryckt == ConsoleKey.B || tryckt == ConsoleKey.M)
            {
                string resultatKommando = motor.HanteraInmatning(tryckt);

                if (motor.IsSelectingItem)
                {
                    VisaForemalsvalet();
                }
                else if (motor.IsInventoryOpen)
                {
                    rtbDisplay.Clear();
                    lstInventory.AccessibleName = "Ryggsäck";
                    VisaRyggsack();
                }
                else
                {
                    VisaRum(resultatKommando);
                }
            }
        }

        private void VisaForemalsvalet()
        {
            btnNav.Visible = false;
            rtbDisplay.Visible = false;
            lstInventory.Visible = true;

            lstInventory.Items.Clear();
            foreach (var sak in motor.ItemsToSelect)
            {
                lstInventory.Items.Add(sak.Namn);
            }

            lstInventory.SelectedIndex = motor.SelectedItemIndex;
            lstInventory.Focus();
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
                if (sak is Klader k) status = k.ArAktiv ? " (påtagen)" : " (i ryggsäcken)";
                else if (sak is Redskap r) status = r.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is AllmanSak a) status = a.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                else if (sak is Livsmedel l) status = l.ArAktiv ? " (i handen)" : " (i ryggsäcken)";
                lstInventory.Items.Add($"{sak.Namn}{status}");
            }

            if (lstInventory.Items.Count > 0)
            {

                if (nuvarandeIndex >= lstInventory.Items.Count)
                    nuvarandeIndex = lstInventory.Items.Count - 1;

                lstInventory.SelectedIndex = nuvarandeIndex != -1 ? nuvarandeIndex : 0;
            }

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

            // rtbDisplay.Focus();
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