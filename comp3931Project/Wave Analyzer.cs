using ScottPlot.Palettes;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static comp3931Project.Wave;
using static System.Windows.Forms.DataFormats;
namespace comp3931Project
{

    [StructLayout(LayoutKind.Sequential)]
    public struct WAVEFORMATEX
    {
        public ushort wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;
    }

    /**
     * Represents the entire Wave Analyzer application
     */
    public unsafe partial class WaveAnalyzer : Form
    {
        static List<WaveWindow> waveWindowList = new List<WaveWindow>();
         static WaveWindow activeWaveWindow;

        /**
         * Purpose: Initializes the Wave Analyzer application
         * 
         * @return: None
         */
        public WaveAnalyzer()
        {
            InitializeComponent();
        }

        /**
         * Purpose: Initializes the base program by loading the wave graph and filter/frequency graph
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void Form1_Load(object sender, EventArgs e)
        {
/*            loadDynamicWaveGraph();
*/        }

        /**
         * Purpose: Loads the wave graph
         * 
         * @return: None
         */
        private void loadDynamicWaveGraph()
        {
            dynamicWaveGraph dynamicWaveGraph = new dynamicWaveGraph();
            dynamicWaveGraph.MdiParent = this;
            dynamicWaveGraph.TopLevel = false;
            dynamicWaveGraph.Show();
            dynamicWaveGraph.Location = new Point(0, 10);
            dynamicWaveGraph.Size = new Size(2035, 800);

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

                /**
       * Purpose: Loads the filter/frequency chart
       * 
       * @return: None
       */
        private void loadFilter()
        {
            Filter filter = new Filter();
            filter.MdiParent = this;
            filter.TopLevel = false;
            filter.Show();
            filter.Location = new Point(0, 420);
            filter.Size = new Size(1033, 381);

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
            WaveWindow wavewindow = new WaveWindow();  
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
                wavewindow.Size = new Size(1033, 372);
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

                loadFilter();

                wavewindow.Show();
                waveWindowList.Add(wavewindow);
                activeWaveWindow = wavewindow;
                setActiveWindow(wavewindow);
            }
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
                Wave wave = activeWaveWindow.getWave();
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


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread recordProgramThread = new Thread(StartRecordProgram);
            Form mdiParentFor = this;
            recordProgramThread.Start();
            Thread recordHandling = new Thread(new ParameterizedThreadStart(RecordingMethod));
            recordHandling.Start(this);
        }

        private void ToolRecordButton_MouseEnter(object sender, EventArgs e)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "StartRecordingProgram");
            waitHandle.Set();
        }

        private void setActiveWindow(WaveWindow w)
        {
            activeWaveWindow = w;
            // TODO send data
        }

        static void RecordingMethod(object parameter)
        {
            bool programOpen = true;

            // Create events with unique names
            WaveAnalyzer formInstance = (WaveAnalyzer)parameter;

            using (EventWaitHandle recordingEnd = new EventWaitHandle(false, EventResetMode.AutoReset, "recordingEnd"))
            using (EventWaitHandle playRecording = new EventWaitHandle(false, EventResetMode.AutoReset, "playRecording"))
            using (EventWaitHandle closeProgram = new EventWaitHandle(false, EventResetMode.AutoReset, "closeProgram"))
            using (EventWaitHandle event2 = new EventWaitHandle(false, EventResetMode.AutoReset, "P2"))
            {
                // Create an array of events
                WaitHandle[] eventsArray = { recordingEnd, playRecording, closeProgram };

                // Wait for any event to be signaled

                while (programOpen)
                {
                    int signaledEventIndex = WaitHandle.WaitAny(eventsArray);
                    // Process the signaled event
                    switch (signaledEventIndex)
                    {
                        case 0:
                            Debug.WriteLine("Recording ended, passing data");

                            IntPtr pByteData = getPSaveBuffer();
                            uint pByteLength = getDwDataLength();
                            byte[] temp = new byte[pByteLength];

                            unsafe
                            {
                                byte* bPointer = (byte*)pByteData;
                                for (int i = 0; i < pByteLength; i++)
                                {
                                    temp[i] = bPointer[i];
                                }
                            }
                            IntPtr waveHeaderPtr = getHeaderStructure();
                            WAVEFORMATEX waveFormatEx = Marshal.PtrToStructure<WAVEFORMATEX>(waveHeaderPtr);
                            int intValue;

                            // Check for overflow before converting
                            if (pByteLength <= int.MaxValue) {
                                intValue = (int)pByteLength;
                            } else {
                                intValue = 0; 
                                Console.WriteLine("Warning: ulong value too large to fit into int.");
                            }

                            formInstance.Invoke((MethodInvoker)delegate
                            {
                                Wave waave = new Wave();
                                waave.populateFromRecord(waveFormatEx, temp, intValue);
                                WaveWindow waveWindowRecorded = new WaveWindow();


                                waveWindowRecorded.ChartWave(waave);

                                waveWindowRecorded.MdiParent = formInstance;
                                waveWindowRecorded.TopLevel = false;
                                waveWindowRecorded.Location = new Point(0, 320);
                                waveWindowRecorded.Size = new Size(1035, 300);
                                waveWindowRecorded.Show();
                                formInstance.setActiveWindow(waveWindowRecorded);

                            });
                            break;
                        case 1:
                            Debug.WriteLine("Play Recording");

                            formInstance.Invoke((MethodInvoker)delegate
                            {
                                byte[] arr = activeWaveWindow.getWave().getData();
                                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(arr[0]) * activeWaveWindow.getWave().getDataSize());

                                try
                                {
                                    Marshal.Copy(arr, 0, ptr, activeWaveWindow.getWave().getDataSize());
                                    byte[] data = new byte[activeWaveWindow.getWave().getDataSize()];
                                    Marshal.Copy(ptr, data, 0, activeWaveWindow.getWave().getDataSize());
                                    receiveBufferFromCSharp(ptr, (uint)activeWaveWindow.getWave().getDataSize());
                                }
                                finally
                                {
                                    Marshal.FreeHGlobal(ptr);
                                }
                            });

                            setDwDataLength((uint)activeWaveWindow.getWave().getDataSize());
                            EventWaitHandle dataSent = new EventWaitHandle(false, EventResetMode.AutoReset, "dataToDLL");
                            dataSent.Set();
                            Debug.WriteLine("data sent!");
                         
                            break;
                        case 2:
                            programOpen = false;
                            Debug.WriteLine("Close Program");
                            break;
                        default:
                            Console.WriteLine("Unexpected event");
                            break;
                    }
                    Debug.WriteLine("loop end!");
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
        static extern void setPSaveBuffer(IntPtr p);

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern void receiveBufferFromCSharp(IntPtr p, uint length);

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern int start();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern uint getDwDataLength();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern void setDwDataLength(uint dl);


        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr getHeaderStructure();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern void setDwDataLength(uint dataLength);

    }
}