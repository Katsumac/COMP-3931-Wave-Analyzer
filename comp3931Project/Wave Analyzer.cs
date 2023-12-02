using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace comp3931Project
{
    public partial class WaveAnalyzer : Form
    {
        WaveWindow wavewindow;

        public WaveAnalyzer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            loadDynamicWaveGraph();
            loadDynamicWaveGraph2();
            loadFilter();
            WaveWindow ww = new WaveWindow();
            wavewindow = ww;
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


            Wave wave = new Wave();
            wavewindow.setWave(wave);

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Wav|*.wav";
            openFileDialog1.Title = "Open a Wav File";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                wave.ReadWavFile(openFileDialog1.FileName);

                wavewindow.ChartWave(wave);

                wavewindow.MdiParent = this;
                wavewindow.TopLevel = false;
                // wavewindow.Show();
                wavewindow.Location = new Point(0, 320);
                wavewindow.Size = new Size(1035, 300);
                wavewindow.Show();

                foreach (Control control in this.Controls)
                {
                    MdiClient client = control as MdiClient;
                    if (client != null)
                    {
                        client.BackColor = Color.Blue;
                        break;
                    }
                }

                wavewindow.Show();
            }

           

            //WaveAnalyzerPanel.Controls.Add(wavewindow);
            //wavewindow.TopLevel = false;
            // wavewindow.FormBorderStyle = FormBorderStyle.None;

            //   wavewindow.AutoScroll = true;
            /*  wavewindow.StartPosition = FormStartPosition.Manual;
              wavewindow.Left = 500;
              wavewindow.Top = 500;*/

            // WaveAnalyzer.panel1.Controls.Add(wavewindow);
            //  wavewindow.Location = new Point(0, 0);
            //   wavewindow.Show();
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
            Wave wave = new Wave();
            wave.ReadWavFile("../../../TestWav.wav");

            dynamicWaveGraph waveGraph = new dynamicWaveGraph();
            waveGraph.Show();


        }

        private void saveToAudioFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Wav|*.wav";
            saveFileDialog1.Title = "Save a Wav File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                Wave wave = wavewindow.getWave();
                wave.WriteWavFile(saveFileDialog1.FileName);
            }

                // WaveFileReadWrite.writeFile(WaveFileReadWrite.readFile("../../../music.wav"), ".\\comp3931Project\\music.wav"); //DataID 1634074624

            }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Wav|*.wav";
                openFileDialog.Title = "Save a Wav File";
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


        private void ToolRecordButton_Click(object sender, EventArgs e)
        {
            start();
            IntPtr i = getPSaveBuffer();
            getDwDataLength();

        }




        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern IntPtr getPSaveBuffer();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern bool start();


        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern ulong getDwDataLength();
    }
}