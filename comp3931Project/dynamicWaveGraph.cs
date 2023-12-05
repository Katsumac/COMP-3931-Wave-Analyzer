using System.Windows.Forms.DataVisualization.Charting;

namespace comp3931Project
{
    /**
     * Represents the wave graph
     */
    public partial class dynamicWaveGraph : Form
    {

        private static Series frequency;
        private static double[] xValues;
        private static double[] yValues;
        private static double[] sample;
        private double start;
        private double end;
        private const int pageSize = 10; // initial number of x values seen on the graph
        private const int yAxisMax = 20; // max value for the y axis
        private const int yAxisMin = -20; // min value for the y axis
        private int zoomedYAxisValue = 20; // counter used for zooming

        /**
         * Purpose: Initializes the wave graph
         * 
         * @return: None
         */
        public dynamicWaveGraph()
        {
            InitializeComponent();
        }

        /**
         * Purpose: Creates the graph samples and draws them
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void dynamicWaveGraph_Load(object sender, EventArgs e)
        {
            sample = Calculations.createSamples(30, 8);
            chart1.Series.Clear(); // clear the chart

            // Populate the bar chart chart
            frequency = chart1.Series.Add("Frequency");
            frequency.ChartType = SeriesChartType.Spline;
            populateLineChart(sample, frequency);

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[frequency.ChartArea];
            customizeLineChart(pageSize, filterChartArea);

            chart1.MouseWheel += chart1_MouseWheel;
            chart1.SelectionRangeChanged += Chart_SelectionRangeChanged;
        }

        /**
         * Purpose: Handles the drawing of the graph
         * 
         * @param sample: The samples
         * @param chartLabel: The graph's chart label
         * 
         * @return: None
         */
        public static void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]);
        }

        /**
         * Purpose: Adds customizations to the graph such as scrollbars, zooming, axes, and style
         * 
         * @param pageSize: The initial number of x values seen on the graph
         * @oaram chartArea: A rectangular area on a chart image
         * 
         * @return: None
         */
        private void customizeLineChart(int pageSize, ChartArea chartArea)
        {
            // How much data we want
            chartArea.AxisX.Minimum = 0;

            // Works with Zoomable to allow zooming via highlighting
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.AutoScroll = true; // Enables scrolling

            // How much we see on one page
            chartArea.AxisY.Maximum = yAxisMax;
            chartArea.AxisY.Minimum = yAxisMin;
            
            chartArea.AxisX.ScaleView.Zoom(0, pageSize);
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
                zoomedYAxisValue = 20;
                yAxis.ScaleView.ZoomReset();
                xAxis.ScaleView.Zoom(0, pageSize);
            } else if (e.Delta > 0 && zoomedYAxisValue > 1) { // Zoom in
                xAxis.ScaleView.Zoom(Math.Floor(posXStart), Math.Floor(posXFinish));
                yAxis.ScaleView.Zoom(0, --zoomedYAxisValue);
            }
        }

        /**
         * Purpose: Updates the range selected by the user
         * 
         * @param sender: The object that raised the event
         * @param e: Contains cursor event data
         * 
         * @return: None
         */
        private void Chart_SelectionRangeChanged(object sender, CursorEventArgs e)
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
         * Purpose: Performs DFT when the DFT button is clicked
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void DFTButton_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.getFilterChart().Points.Clear();
            /*applyTriangularWindow();*/
            applyRectangularWindow();
            double[] DFTSamples = Calculations.DFT(sample);
            filter.populateBarChart(DFTSamples, filter.getFilterChart());
            filter.getFilterChart().Color = Color.Green;
            filter.Filter_Load(sender, e);
        }

        /**
         * Purpose: For comparison purposes. Performs nonthreaded DFT when the DFT (sync) button is clicked
         * 
         * @param sender: The object that raised the event
         * @param e: Contains event data
         * 
         * @return: None
         */
        private void DFTSyncButton_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.getFilterChart().Points.Clear();
            /*applyTriangularWindow();*/
            applyRectangularWindow();
            double[] DFTSamples = Calculations.DFTSync(sample);
            filter.populateBarChart(DFTSamples, filter.getFilterChart());
            filter.getFilterChart().Color = Color.Green;
            filter.Filter_Load(sender, e);
        }

        /**
         * Purpose: Handles copy/cut/paste from the ctrl + C, X and V keys
         * 
         * @param sender: The object that raised the event
         * @param e: Contains key event data
         * 
         * @return: None
         */
        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                setValuesToClipboard(false);
            }
            else if (e.KeyCode == Keys.X && e.Control)
            {
                setValuesToClipboard(true);
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                pasteLineChart(xValues, retrieveData(), frequency);
            }
        }

        /**
         * Purpose: Gets the values from the selection range and copies them to the clipboard
         * 
         * @param isCut: bool that represents whether the incoming values were cut (true) or copied (false)
         * 
         * @return: None
         */
        private void setValuesToClipboard(bool isCut)
        {
            int range = (int)(end - start) + 1;
            xValues = new double[range];
            yValues = new double[range];
            for (int i = 0; i < range; i++)
            {
                xValues[i] = frequency.Points[(int)(start + i)].GetValueByName("X");
                yValues[i] = frequency.Points[(int)(start + i)].GetValueByName("Y");

                if (isCut)
                {
                    sample[i] = 0;
                }
            }
            copySelection(yValues);
        }

        /**
         * Purpose: Sets the data as a string and saves it to the clipboard
         * 
         * @param yValues: The y values the user has selected from the wave graph
         * 
         * @return: None
         */
        private void copySelection(double[] yValues)
        {
            string data = string.Join(",", yValues);
            Clipboard.SetText(data);
        }

        /**
         * Purpose: Takes the selected values and pastes them on the wave graph
         * 
         * @param xValues: The x values the user has selected from the wave graph
         * @param yValues: The y values the user has selected from the wave graph
         * @param chartLabel: Represents a set of data points
         * 
         * @return: None
         */
        private void pasteLineChart(double[] xValues, double[] yValues, Series chartLabel)
        {
            int yIndex = 0;
            for (int i = 0; i < xValues.Length; i++)
            {
                sample[i] = yValues[yIndex];
                yIndex++;
            }
            chartLabel.Points.Clear();
            populateLineChart(sample, chartLabel);
        }

        /**
         * Purpose: Retrieves the selected values from the clipboard and copies them to a double array
         * 
         * @return: The user-selected values from the clipboard
         */
        private double[] retrieveData()
        {
            string copiedDataString = Clipboard.GetText();
            string[] copiedDataStringArr = copiedDataString.Split(',');
            double[] actualData = new double[copiedDataStringArr.Length];

            for (int i = 0; i < copiedDataStringArr.Length; i++)
            {
                actualData[i] = Convert.ToDouble(copiedDataStringArr[i]);
            }
            return actualData;
        }

        /**
         * Purpose: Returns the sample array
         * 
         * @return: The sample array
         */
        public static double[] getSample()
        {
            return sample;
        }

        /**
         * Purpose: Sets sample to a new double array
         * 
         * @param newSample: The new sample array
         * 
         * @return: None
         */
        public static void setSample(double[] newSample)
        {
            sample = newSample;
        }

        /**
         * Purpose: Returns the chart label
         * 
         * @return: The chart label
         */
        public static Series getChartLabel()
        {
            return frequency;
        }

        /**
         * Purpose: Performs the triangle windowing on the samples
         * 
         * @return: None
         */
        private void applyTriangularWindow()
        {
            int N = sample.Length;
            for (int n = 0; n < N; n++)
            {
                sample[n] *= 1.0 - Math.Abs((n - N / 2) / (N / 2));
            }
        }

        /**
         * Purpose: Performs rectangle windowing on the samples
         * 
         * @return: None
         */
        private void applyRectangularWindow()
        {
            int N = sample.Length;
            for (int n = 0; n < N; n++)
            {
                sample[n] *= 1;
            }
        }
    }
}