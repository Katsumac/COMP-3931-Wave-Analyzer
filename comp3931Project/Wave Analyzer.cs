using ScottPlot.Palettes;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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
            filter.Size = new Size(1053, 372);

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
         * Purpose: Prompts user to select a wav file. Opens a wave window with the waveform of the wav file and a filter/frequency graph
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void FileOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WaveWindow wavewindow = new WaveWindow();  
            Wave wave = new Wave();
            wavewindow.setWave(wave);

            OpenFileDialog openFileDialog1 = new OpenFileDialog(); // Opens file menu
            openFileDialog1.Filter = "Wav|*.wav";
            openFileDialog1.Title = "Open a Wav File";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                wave.ReadWavFile(openFileDialog1.FileName); // Read the wav file

                wavewindow.ChartWave(wave); // Draw the wave on the graph

                wavewindow.MdiParent = this;
                wavewindow.TopLevel = false;
                wavewindow.Location = new Point(0, 320);
                wavewindow.Size = new Size(1053, 372);
                wavewindow.Text = openFileDialog1.SafeFileName;
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

        /**
         * Purpose: Saves the wave in WaveWindow into a new Wav file
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
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
                activeWaveWindow.Text = Path.GetFileName(saveFileDialog1.FileName);
            }
        }

        /**
         * Purpose: Starts another record program thread
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread recordProgramThread = new Thread(StartRecordProgram);
            Form mdiParentFor = this;
            recordProgramThread.Start();
            Thread recordHandling = new Thread(new ParameterizedThreadStart(RecordingMethod));
            recordHandling.Start(this);
        }

        /**
         * Purpose: Sets an mouse event handler when hovering over the tool record button
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void ToolRecordButton_MouseEnter(object sender, EventArgs e)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "StartRecordingProgram");
            waitHandle.Set();
        }

        /**
         * Purpose: Sets the active window
         * 
         * @param w: A WaveWindow object
         * 
         * @return: None
         */
        private void setActiveWindow(WaveWindow w)
        {
            activeWaveWindow = w;
            // TODO send data
        }

        /**
         * Purpose: Contains the logic for recording from the recorderDLL.dll
         * 
         * @param parameter: An object parameter. Is passed to a thread
         */
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


                                waveWindowRecorded.ChartWave(wave);

                                waveWindowRecorded.MdiParent = formInstance;
                                waveWindowRecorded.TopLevel = false;
                                waveWindowRecorded.Location = new Point(0, 320);
                                waveWindowRecorded.Size = new Size(1035, 300);
                                waveWindowRecorded.Show();
                                formInstance.setActiveWindow(waveWindowRecorded);
                                formInstance.loadFilter();
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

        /**
         * Purpose: Runs the start function from the recorderDLL.dll
         * 
         * @return: None
         */
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
        public static extern IntPtr getHeaderStructure();

        [DllImport("../../../recorderDLL.dll", CharSet = CharSet.Auto)]
        static extern void setDwDataLength(uint dataLength);

    }
}