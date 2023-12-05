using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Drawing.Printing;

namespace comp3931Project
{
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
         * Initializes the filter/frequency graph
         */
        public Filter()
        {
            InitializeComponent();
        }

        /**
         * Loads a blank filter/frequency graph
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
        * Handles the drawing of the graph
        */
        public void populateBarChart(double[] A, Series chartLabel)
        {
            for (int i = 0; i < A.Length; i++)
            {
                chartLabel.Points.AddXY(i, A[i]);
            }
            const int pageSize = 10;
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            customizeBarChart(pageSize, filterChartArea, filterChart);
        }

        /**
         * Adds customizations to the graph such as scrollbars, zooming, axes, and style
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
         * Performs zooming when the mouse wheel is scrolled
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
         * Returns the chart label
         */
        public Series getChartLabel()
        {
            filterChart = chart1.Series.Add("Frequency");
            return filterChart;
        }

        /**
         * Performs filtering and updates wave graph when the filter button has been clicked
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
         * Returns the filter/frequency chart
         */
        public Series getFilterChart()
        {
            return filterChart;
        }

        /**
         * Updates the range selected by the user
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
         * For comparison purposes. Performs nonthreaded filtering
         */
        private void filterSyncButton_Click(object sender, EventArgs e)
        {
            double[] samples = dynamicWaveGraph.getSample();
            Series freq = dynamicWaveGraph.getChartLabel();
            Calculations.createLowPassFilter(samples.Length, (int)end);
            Calculations.convolveSync(samples);
            samples = dynamicWaveGraph.getSample();
            freq.Points.Clear();
            dynamicWaveGraph.populateLineChart(samples, freq);
            dynamicWaveGraph waveGraph = new dynamicWaveGraph();
            waveGraph.Update();
        }

        /**
         * For comparison purposes. Performs threaded inverse DFT
         */
        private void iDFTButton_Click(object sender, EventArgs e)
        {
            double[] testArray = new double[30];
            Calculations.inverseDFT(testArray.Length, testArray);
        }

        /**
         * For comparison purposes. Performs nonthreaded inverse DFT
         */
        private void iDFTSyncButton_Click(object sender, EventArgs e)
        {
            double[] testArray = new double[30];
            Calculations.inverseDFTSync(testArray.Length, testArray);
        }
    }
}
