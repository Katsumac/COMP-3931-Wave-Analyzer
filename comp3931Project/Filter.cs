using System.Windows.Forms.DataVisualization.Charting;

namespace comp3931Project
{
    /**
     * Represents the filter/frequency chart
     */
    public partial class Filter : Form
    {
        private double start;
        private double end;
        private static Series filterChart;
        private const int pageSize = 50;

        /**
         * Purpose: Initializes the filter/frequency graph
         * 
         * @return: None
         */
        public Filter()
        {
            InitializeComponent();
        }

        /**
         * Purpose: Loads a blank filter/frequency graph
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        public void Filter_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            filterChart = chart1.Series.Add("Amplitude");

            filterChart.Points.AddXY(1, 1);
            filterChart.Color = Color.Transparent;

            chart1.Update();

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            filterChartArea.AxisX.Title = "Frequency (Hz)";
            filterChartArea.AxisY.Title = "Amplitude";

            customizeBarChart(pageSize, filterChartArea, filterChart);

            chart1.MouseWheel += chart1_MouseWheel;

            chart1.SelectionRangeChanged += chart1_SelectionRangeChanged;
        }

        /**
        * Purpose: Handles the drawing of the graph
        * 
        * @param A: The amplitudes
        * @param S: The sample rate
        * @param chartLabel: Represents a set of data points
        * 
        * @return: None
        */
        public void populateBarChart(double[] A, int S, Series chartLabel)
        {
            chartLabel.Points.Clear();
            A[0] = 0;
            for (int i = 0; i < A.Length; i++)
            {
                chartLabel.Points.AddXY(i * S / A.Length, A[i]);
            }
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            customizeBarChart(pageSize, filterChartArea, filterChart);
            chart1.Update();
        }

        /**
         * Purpose: Adds customizations to the graph such as scrollbars, zooming, axes, and style
         * 
         * @param pageSize: The initial number of x values seen on the graph
         * @oaram chartArea: A rectangular area on a chart image
         * @param filterChart: Stores data and properties of the filter chart
         * 
         * @return: None
         */
        private void customizeBarChart(int pageSize, ChartArea chartArea, Series filterChart)
        {
            chartArea.AxisX.Minimum = 0; // Minimum value on the x axis

            // Works with Zoomable to allow zooming via highlighting
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.AutoScroll = true; // Enables scrolling

            // How much we see on one page

            chartArea.AxisX.ScaleView.Zoomable = false; // Cannot zoom by highlighting
            filterChart["PixelPointWidth"] = "16";
            chartArea.AxisX.Interval = 5;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // Sets the thumb style
            chartArea.AxisX.ScaleView.SmallScrollSize = pageSize; // Small scrolling size
        }

        /**
         * Context frame that stores x and y values of where we are zooming
         */
        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XFinish { get; set; }
            public double YStart { get; set; }
            public double YFinish { get; set; }
        }

        private readonly Stack<ZoomFrame> _zoomFrames = new Stack<ZoomFrame>();

        /**
         * Purpose: Performs zooming when the mouse wheel is scrolled
         * 
         * @param sender: The object that raised the event
         * @param e: Contains mouse event data
         * 
         * @return: None
         */
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0)
                {
                    if (0 < _zoomFrames.Count)
                    {
                        var frame = _zoomFrames.Pop();
                        if (_zoomFrames.Count == 0)
                        {
                            xAxis.ScaleView.ZoomReset();
                            yAxis.ScaleView.ZoomReset();
                        }
                        else
                        {
                            xAxis.ScaleView.Zoom(frame.XStart, frame.XFinish);
                            yAxis.ScaleView.Zoom(frame.YStart, frame.YFinish);
                        }
                    }
                }
                else if (e.Delta > 0)
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    _zoomFrames.Push(new ZoomFrame { XStart = xMin, XFinish = xMax, YStart = yMin, YFinish = yMax });

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }

            }
            catch { }
        }

        /**
         * Purpose: Performs filtering and updates wave graph when the filter button has been clicked
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void FilterButton_Click(object sender, EventArgs e)
        {
            double[] samples = WaveWindow.getSample();
            Series freq = WaveWindow.getChartLabel();
            Calculations.createLowPassFilter(samples.Length, (int)end);
            Calculations.convolve(samples);
            samples = WaveWindow.getSample();
            freq.Points.Clear();
            WaveWindow.populateLineChart(samples, freq);
            WaveWindow waveGraph = new WaveWindow();
            waveGraph.Update();
        }

        /**
         * Purpose: Returns the filter/frequency chart
         * 
         * @return: The filter/frequency chart
         */
        public Series getFilterChart()
        {
            return filterChart;
        }

        /**
         * Purpose: Updates the range selected by the user
         * 
         * @param sender: The object that raised the event
         * @param e: Contains cursor event data
         * 
         * @return: None
         */
        private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            start = e.NewSelectionStart;
            end = e.NewSelectionEnd;

            if (start > end)
            {
                double temp = start;
                start = end;
                end = temp;
            }
        }

        /**
         * Purpose: For comparison purposes. Performs nonAssembly filtering
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void filterSyncButton_Click(object sender, EventArgs e)
        {
            double[] samples = WaveWindow.getSample();
            Series freq = WaveWindow.getChartLabel();
            Calculations.createLowPassFilter(samples.Length, (int)end);
            Calculations.convolveSync(samples);
            samples = WaveWindow.getSample();
            freq.Points.Clear();
            WaveWindow.populateLineChart(samples, freq);
            WaveWindow waveGraph = new WaveWindow();
            waveGraph.Update();
        }

        /**
         * Purpose: For comparison purposes. Performs threaded inverse DFT
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void iDFTButton_Click(object sender, EventArgs e)
        {
            double[] samples = Calculations.inverseDFT(Calculations.getAmplitudes().Length, Calculations.getAmplitudes());
            WaveWindow.setSample(samples);
            Series freq = WaveWindow.getChartLabel();
            freq.Points.Clear();
            WaveWindow.populateLineChart(samples, freq);
            WaveWindow waveGraph = new WaveWindow();
            waveGraph.Update();
        }

        /**
         * Purpose: For comparison purposes. Performs nonthreaded inverse DFT
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void iDFTSyncButton_Click(object sender, EventArgs e)
        {
            double[] samples = Calculations.inverseDFTSync(Calculations.getAmplitudes().Length, Calculations.getAmplitudes());
            Series freq = WaveWindow.getChartLabel();
            freq.Points.Clear();
            WaveWindow.populateLineChart(samples, freq);
            WaveWindow waveGraph = new WaveWindow();
            waveGraph.Update();
        }
    }
}