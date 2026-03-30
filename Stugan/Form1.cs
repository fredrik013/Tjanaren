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

            motor = new Spelmotor();

            rtbDisplay = new RichTextBox();
            rtbDisplay.Dock = DockStyle.Fill;
            rtbDisplay.ReadOnly = true;
            rtbDisplay.Font = new Font("Consolas", 12); // Consolas känns lite "retro/konsol"

            string startHalsning = motor.HamtaStartText();

            this.Controls.Add(rtbDisplay);


            // Skriv ut den första hälsningen direkt
            rtbDisplay.AppendText("Systemet startat...\n");
            rtbDisplay.AppendText("Välkommen till Stugan Lab.");
            rtbDisplay.AppendText(startHalsning + "\n");
            this.KeyPreview = true; // Gör att fönstret ser tangenttryck först
            this.KeyDown += Form1_KeyDown; // Koppla ihop trycket med en metod
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
