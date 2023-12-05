using System.Diagnostics;
using System.Reflection.Metadata;
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
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "StartRecordingProgram");
            waitHandle.Set();
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
            Thread recordProgramThread = new Thread(StartRecordProgram);
            Form mdiParentFor = this;
            recordProgramThread.Start();
            Thread recordHandling = new Thread(new ParameterizedThreadStart(RecordingMethod));
            recordHandling.Start(this);
        }
        static void RecordingMethod(object parameter)
        {
            bool programOpen = true;

            // Create events with unique names
            WaveAnalyzer formInstance = (WaveAnalyzer)parameter;

            // Use Invoke to set child window on the UI thread
           


            using (EventWaitHandle recordingEnd = new EventWaitHandle(false, EventResetMode.AutoReset, "recordingEnd"))
            using (EventWaitHandle playRecording = new EventWaitHandle(false, EventResetMode.AutoReset, "playRecording"))
            using (EventWaitHandle closeProgram = new EventWaitHandle(false, EventResetMode.AutoReset, "playRecording"))
            using (EventWaitHandle event2 = new EventWaitHandle(false, EventResetMode.AutoReset, "P2"))
            {
                // Create an array of events
                WaitHandle[] eventsArray = { recordingEnd, playRecording, closeProgram };

                // Wait for any event to be signaled

                while (true)
                {
                    int signaledEventIndex = WaitHandle.WaitAny(eventsArray);
                    // Process the signaled event
                    switch (signaledEventIndex)
                    {
                        case 0:
                            Debug.WriteLine("Recording ended, passing data");
                            IntPtr pByteData = getPSaveBuffer();
                            uint pByteLength = getDwDataLength();
                     
                            int intValue;

                            // Check for potential overflow before converting
                            if (pByteLength <= int.MaxValue)
                            {
                                intValue = (int)pByteLength;
                            }
                            else
                            {
                                // Handle the case where the ulong value is too large to fit into an int
                                // You might want to throw an exception, use a default value, or handle it in some other way
                                // For example:
                                intValue = 0; // or any other appropriate default value
                                Console.WriteLine("Warning: ulong value too large to fit into int.");
                            }


                            byte[] byteArray = new byte[intValue];
                            Marshal.Copy(pByteData, byteArray, 0, intValue);
                            formInstance.Invoke((MethodInvoker)delegate
                            {

                            Wave waave = new Wave();
                            waave.readByteArr(byteArray);
                            WaveWindow waveWindowRecorded = new WaveWindow();


                            waveWindowRecorded.ChartWave(waave);

                        waveWindowRecorded.MdiParent = formInstance;
                            waveWindowRecorded.TopLevel = false;
                            waveWindowRecorded.Location = new Point(0, 320);
                            waveWindowRecorded.Size = new Size(1035, 300);
                            waveWindowRecorded.Show();

                            });
                            break;
                        case 1:
                            Debug.WriteLine("Play Recording");
                            break;
                        case 2:
                            programOpen = false;
                            Debug.WriteLine("Close Program");
                            break;
                        default:
                            Console.WriteLine("Unexpected event");
                            break;
                    }
                }
            }
        }

        static void StartRecordProgram()
        {
            start();
        }



        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern IntPtr getPSaveBuffer();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern int start();


        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern uint getDwDataLength();
    }
}