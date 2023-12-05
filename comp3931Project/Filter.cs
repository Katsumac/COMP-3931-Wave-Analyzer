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
        private const int pageSize = 20;
        private const int yAxisMax = 10;
        private const int yAxisMin = 0;
        private int zoomedYAxisValue = 10;

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
            filterChart = chart1.Series.Add("Frequency");

            filterChart.Points.AddXY(1, 1);
            filterChart.Color = Color.Transparent;

            chart1.Update();

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            customizeBarChart(pageSize, filterChartArea, filterChart);

            chart1.MouseWheel += chart1_MouseWheel;

            chart1.SelectionRangeChanged += chart1_SelectionRangeChanged;
        }

        /**
        * Purpose: Handles the drawing of the graph
        * 
        * @param A: The amplitudes
        * @param chartLabel: Represents a set of data points
        * 
        * @return: None
        */
        public void populateBarChart(double[] A, Series chartLabel)
        {
            for (int i = 0; i < A.Length; i++)
            {
                chartLabel.Points.AddXY(i, A[i]);
            }
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            customizeBarChart(pageSize, filterChartArea, filterChart);
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
            chartArea.AxisY.Maximum = yAxisMax;
            chartArea.AxisY.Minimum = yAxisMin;
            
            chartArea.AxisX.ScaleView.Zoom(0, pageSize);
            filterChart["PixelPointWidth"] = "16";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // Sets the thumb style
            chartArea.AxisX.ScaleView.SmallScrollSize = pageSize; // Small scrolling size
        }

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
            Chart chart = (Chart)sender;
            Axis xAxis = chart.ChartAreas[0].AxisX;
            Axis yAxis = chart.ChartAreas[0].AxisY;
            double xMin = xAxis.ScaleView.ViewMinimum; // Get minimum x axis value
            double xMax = xAxis.ScaleView.ViewMaximum; // Get maximum x axis value
            double posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 10;
            double posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 10;

            if (e.Delta < 0) { // Zoom out
                zoomedYAxisValue = 10;
                yAxis.ScaleView.ZoomReset();
                xAxis.ScaleView.Zoom(0, pageSize);
            } else if (e.Delta > 0 && zoomedYAxisValue > 1) { // Zoom in
                xAxis.ScaleView.Zoom(Math.Floor(posXStart), Math.Floor(posXFinish));
                yAxis.ScaleView.Zoom(0, --zoomedYAxisValue);
            }
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
            double[] samples = dynamicWaveGraph.getSample();
            Series freq = dynamicWaveGraph.getChartLabel();
            Calculations.createLowPassFilter(samples.Length, (int)end);
            Calculations.convolve(samples);
            samples = dynamicWaveGraph.getSample();
            freq.Points.Clear();
            dynamicWaveGraph.populateLineChart(samples, freq);
            dynamicWaveGraph waveGraph = new dynamicWaveGraph();
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
            double[] samples = dynamicWaveGraph.getSample();
            Series freq = dynamicWaveGraph.getChartLabel();
            Calculations.createLowPassFilter(samples.Length, (int)end);
            Calculations.convolveNonAsm(samples);
            samples = dynamicWaveGraph.getSample();
            freq.Points.Clear();
            dynamicWaveGraph.populateLineChart(samples, freq);
            dynamicWaveGraph waveGraph = new dynamicWaveGraph();
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
            double[] testArray = new double[30];
            Calculations.inverseDFT(testArray.Length, testArray);
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
            double[] testArray = new double[30];
            Calculations.inverseDFTSync(testArray.Length, testArray);
        }
    }
}