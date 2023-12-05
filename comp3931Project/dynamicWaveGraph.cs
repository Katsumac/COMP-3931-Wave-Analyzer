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
using System.Reflection.Metadata.Ecma335;

namespace comp3931Project
{
    public partial class dynamicWaveGraph : Form
    {

        private static Series frequency;
        private static double[] xValues;
        private static double[] yValues;
        private static double[] sample;
        private double start;
        private double end;

        /**
         * Initializes the wave graph
         */
        public dynamicWaveGraph()
        {
            InitializeComponent();
        }

        /**
         * Creates the graph samples and draws them
         */
        private void dynamicWaveGraph_Load(object sender, EventArgs e)
        {
            sample = Calculations.createSamples(30, 8);
            const int pageSize = 10;
            chart1.Series.Clear(); // clear the chart

            // Populate the bar chart chart
            frequency = chart1.Series.Add("Frequency");
            frequency.ChartType = SeriesChartType.Spline;
            populateLineChart(sample, frequency);

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[frequency.ChartArea];
            customizeLineChart(pageSize, filterChartArea, sample);

            chart1.MouseWheel += chart1_MouseWheel;
            chart1.SelectionRangeChanged += Chart_SelectionRangeChanged;
        }

        /**
         * Handles the drawing of the graph
         */
        public static void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]);
        }

        /**
         * Adds customizations to the graph such as zooming, axes, and style
         */
        private void customizeLineChart(int pageSize, ChartArea chartArea, double[] sample)
        {
            // How much data we want
            chartArea.AxisX.Minimum = 0;

            // Works with Zoomable to allow zooming via highlighting
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;

            chartArea.CursorX.AutoScroll = true; // Enables scrolling

            // How much we see on one page
            chartArea.AxisY.Maximum = 5;
            chartArea.AxisY.Minimum = -5;
            chartArea.AxisX.ScaleView.Zoom(0, pageSize);
            chartArea.AxisX.Interval = 1;

            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll; // Sets the thumb style

            chartArea.AxisX.ScaleView.SmallScrollSize = pageSize; // Small scrolling size
        }

        /**
         * Returns the x values of the wave graph
         */
        public double[] getXValues()
        {
            return xValues;
        }

        /**
         * Returns the y values of the wave graph
         */
        public double[] getYValues()
        {
            return yValues;
        }

        /**
         * Performs zooming when the mouse wheel is scrolled
         */
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;
            Axis yAxis = chart.ChartAreas[0].AxisY;

            if (e.Delta < 0) { // Zoom out
                yAxis.Maximum++;
                yAxis.Minimum--;
            } else if (e.Delta > 0 && yAxis.Maximum > 1) { // Zoom in
                yAxis.Maximum--;
                yAxis.Minimum++;
            }
        }

        /**
         * Updates the range selected by the user
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
         * Performs DFT when the DFT button is clicked
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
         * For comparison purposes. Performs nonthreaded DFT when the DFT (sync) button is clicked
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
         * Handles copy/cut/paste from the ctrl + C, X and V keys
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
         * Gets the values from the selection range and copies them to the clipboard
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
         * Sets the data as a string and saves it to the clipboard
         */
        private void copySelection(double[] yValues)
        {
            string data = string.Join(",", yValues);
            Clipboard.SetText(data);
        }

        /**
         * Takes the selected values and pastes them on the wave graph
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
         * Retrieves the selected values from the clipboard and copies them to a double array
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
         * Returns the sample array
         */
        public static double[] getSample()
        {
            return sample;
        }

        /**
         * Sets sample to a new double array
         */
        public static void setSample(double[] newSample)
        {
            sample = newSample;
        }

        /**
         * Returns the chart label
         */
        public static Series getChartLabel()
        {
            return frequency;
        }

        /**
         * Performs the triangle windowing on the samples
         */
        public void applyTriangularWindow()
        {
            int N = sample.Length;
            for (int n = 0; n < N; n++)
            {
                sample[n] *= 1.0 - Math.Abs((n - N / 2) / (N / 2));
            }
        }

        /**
         * Performs rectangle windowing on the samples
         */
        public void applyRectangularWindow()
        {
            int N = sample.Length;
            for (int n = 0; n < N; n++)
            {
                sample[n] *= 1;
            }
        }
    }
}
