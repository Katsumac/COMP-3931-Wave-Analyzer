using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace comp3931Project
{
    public partial class WaveAnalyzer : Form
    {
        public WaveAnalyzer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            loadFilter();   
        }

        private void loadFilter()
        {

            Filter filter = new Filter();
            filter.MdiParent = this;
            filter.TopLevel = false;
            filter.Show();
            filter.Location = new Point(0, 300);
            filter.Size = new Size(1000, 330);

            foreach (Control control in this.Controls)
            {
                MdiClient client = control as MdiClient;
                if (client != null)
                {
                    client.BackColor = Color.Blue;
                    break;
                }
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void surpriseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveWindow wavewindow = new WaveWindow() { TopLevel = false, TopMost = true };
            wavewindow.FormBorderStyle = FormBorderStyle.None;
            WaveAnalyzerPanel.Controls.Add(wavewindow);
            //wavewindow.TopLevel = false;
            // wavewindow.FormBorderStyle = FormBorderStyle.None;

            //   wavewindow.AutoScroll = true;
            /*  wavewindow.StartPosition = FormStartPosition.Manual;
              wavewindow.Left = 500;
              wavewindow.Top = 500;*/

            // WaveAnalyzer.panel1.Controls.Add(wavewindow);
            //  wavewindow.Location = new Point(0, 0);
            wavewindow.Show();
        }


        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFilter();

        private void WaveAnalyzerPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}