using Tjanaren.Core;
using Tjanaren.Modeller.Saker;

namespace Tjanaren
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        ListBox lstInventory;
        ListBox lstRoomItems;
        ListBox lstMaintenance;
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
            lstMaintenance = new ListBox
            {
                Dock = DockStyle.Fill,
                Visible = false,
                Font = new Font("Consolas", 12),
                TabIndex = 5,
                AccessibleName = "Underhåll"
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
            this.Controls.Add(lstMaintenance);
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
            if (motor.IsInventoryOpen && lstInventory.Visible && lstInventory.Focused)
            {
                if (tryckt == ConsoleKey.Escape || tryckt == ConsoleKey.I)
                {
                    motor.HanteraInmatning(ConsoleKey.Escape);

                    var rum = motor._spelare.NuvarandeRum;
                    string rumsInfo = $"{rum.Namn}\n{rum.Beskrivning}";
                    VisaRum(rumsInfo);
                    btnNav.Focus();
                    return;
                }

                motor.Ryggsack.MarkeratIndex = lstInventory.SelectedIndex;
                string resultatInv = motor.HanteraInmatning(tryckt);

                if (!string.IsNullOrEmpty(resultatInv))
                {
                    // rtbDisplay.Clear();
                    rtbDisplay.AppendText(resultatInv);
                    lstInventory.AccessibleName = resultatInv;
                }
                VisaRyggsack();
                return;
            }

            if (motor.IsRoomMenuOpen &&
                !(rtbDisplay.Focused &&
                  (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                   e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)))
            {
                ConsoleKey menyTangent = tryckt;

                if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
                {
                    menyTangent = (ConsoleKey)((int)ConsoleKey.D1 + (int)(e.KeyCode - Keys.D1));
                }
                else if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9)
                {
                    menyTangent = (ConsoleKey)((int)ConsoleKey.D1 + (int)(e.KeyCode - Keys.NumPad1));
                }

                string resultat = motor.HanteraInmatning(menyTangent);

                if (!string.IsNullOrEmpty(resultat))
                {
                    rtbDisplay.Text = resultat;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }


            if (tryckt == ConsoleKey.C && rtbDisplay.Visible)
            {
                rtbDisplay.Clear();
                rtbDisplay.Focus();

                e.Handled = true;
                e.SuppressKeyPress = true;
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
                    // rtbDisplay.Clear();
                    lstInventory.AccessibleName = "Inventarie";
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
            lstInventory.Dock = DockStyle.Fill;
            rtbDisplay.Dock = DockStyle.Bottom;
            rtbDisplay.Height = 100;
            rtbDisplay.Visible = true;
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

        private void VisaUnderhall(IEnumerable<string> alternativ)
        {
            btnNav.Visible = false;
            lstInventory.Visible = false;

            lstMaintenance.Dock = DockStyle.Fill;
            rtbDisplay.Dock = DockStyle.Bottom;
            rtbDisplay.Height = 100;

            rtbDisplay.Visible = true;
            lstMaintenance.Visible = true;

            lstMaintenance.Items.Clear();

            foreach (string alternativText in alternativ)
            {
                lstMaintenance.Items.Add(alternativText);
            }

            if (lstMaintenance.Items.Count > 0)
                lstMaintenance.SelectedIndex = 0;

            lstMaintenance.Focus();
        }

        private void VisaRum(string resultat)
        {
            lstInventory.Visible = false;
            btnNav.Visible = true;
            rtbDisplay.Visible = true;
            lstInventory.Dock = DockStyle.Fill;
            rtbDisplay.Dock = DockStyle.Fill;
            // rtbDisplay.Clear();

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