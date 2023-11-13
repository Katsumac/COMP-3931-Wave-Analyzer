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

            loadDynamicWaveGraph();
            loadDynamicWaveGraph2();
            loadFilter();
        }

        private void loadDynamicWaveGraph()
        {
            dynamicWaveGraph dynamicWaveGraph = new dynamicWaveGraph();
            dynamicWaveGraph.MdiParent = this;
            dynamicWaveGraph.TopLevel = false;
            dynamicWaveGraph.Show();
            dynamicWaveGraph.Location = new Point(0, 10);
            dynamicWaveGraph.Size = new Size(1035, 300);

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

        private void loadDynamicWaveGraph2()
        {
            dynamicWaveGraph2 dynamicWaveGraph2 = new dynamicWaveGraph2();
            dynamicWaveGraph2.MdiParent = this;
            dynamicWaveGraph2.TopLevel = false;
            dynamicWaveGraph2.Show();
            dynamicWaveGraph2.Location = new Point(0, 320);
            dynamicWaveGraph2.Size = new Size(1035, 300);

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

        private void loadFilter()
        {

            Filter filter = new Filter();
            filter.MdiParent = this;
            filter.TopLevel = false;
            filter.Show();
            filter.Location = new Point(0, 630);
            filter.Size = new Size(1035, 300);

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

        private void surpriseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveWindow wavewindow = new WaveWindow() { TopLevel = false, TopMost = true };
            wavewindow.FormBorderStyle = FormBorderStyle.None;
            //WaveAnalyzerPanel.Controls.Add(wavewindow);
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
        }
        private void WaveAnalyzerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void audioFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveFileReadWrite.readFile(".\\comp3931Project\\music.wav");
        }

        private void saveToAudioFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveFileReadWrite.writeFile(WaveFileReadWrite.readFile(".\\comp3931Project\\music.wav"), ".\\comp3931Project\\music.wav");
        }

        private void waveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dynamicWaveGraph waveGraph = new dynamicWaveGraph();
            waveGraph.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

    }
}