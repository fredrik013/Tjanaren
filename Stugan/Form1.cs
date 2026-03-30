using Stugan.Core;

namespace Stugan
{
    public partial class Form1 : Form
    {
        RichTextBox rtbDisplay;
        Spelmotor motor;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Stugan";
            motor = new Spelmotor();

            rtbDisplay = new RichTextBox();
            rtbDisplay.Dock = DockStyle.Fill;
            rtbDisplay.ReadOnly = true;
            rtbDisplay.Font = new Font("Consolas", 12);
            this.Controls.Add(rtbDisplay);

            string startHalsning = motor.HamtaStartText();

            // Skriv ut den första hälsningen - lade till \n för tydlighet
            rtbDisplay.AppendText("Systemet startat...\n");
            rtbDisplay.AppendText("Välkommen till Stugan Lab.\n\n"); // Dubbel radbrytning här
            rtbDisplay.AppendText(startHalsning + "\n");

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            // Se till att markören hamnar i rutan direkt så JAWS hittar texten
            rtbDisplay.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Konvertera Windows-tangent till ConsoleKey
            ConsoleKey tryckt = (ConsoleKey)e.KeyCode;

            // Kör den gamla bekanta metoden
            string resultat = motor.HanteraInmatning(tryckt);

            // Visa resultatet
            rtbDisplay.Clear();
            rtbDisplay.AppendText(resultat);
        }
    }
}
