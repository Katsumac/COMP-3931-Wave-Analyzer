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
            const int pageSize = 10;
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

        private void customizeBarChart(int pageSize, ChartArea chartArea, Series filterChart)
        {
            // How much data we want
            chartArea.AxisX.Minimum = 0;

            // Enables scrolling
            chartArea.CursorX.AutoScroll = true;

            // How much we see on one page
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.AxisX.ScaleView.Zoom(0, pageSize);
            filterChart["PixelPointWidth"] = "16";
            chartArea.AxisX.Interval = 1;

            // Works with Zoomable to allow zooming via highlighting
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.ScrollBar.IsPositionedInside = true;

            // Sets the thumb style
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // Small scrolling size
            chartArea.AxisX.ScaleView.SmallScrollSize = pageSize;
        }

        /*      For bar chart scrolling*/
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            var xMin = xAxis.ScaleView.ViewMinimum;
            var xMax = xAxis.ScaleView.ViewMaximum;
            var yMax = yAxis.ScaleView.ViewMaximum;

            var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
            var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;

            if (e.Delta < 0)
            {
                xAxis.ScaleView.Zoom(-5, 10);
                yAxis.ScaleView.ZoomReset();
            }
            else if (e.Delta > 0)
            {
                xAxis.ScaleView.Zoom(Math.Floor(posXStart), Math.Floor(posXFinish));
                yAxis.ScaleView.Zoom(0, yMax - 1);
            }
        }

        public Series getChartLabel()
        {
            filterChart = chart1.Series.Add("Frequency");
            return filterChart;
        }

        // Work on this
        private void FilterButton_Click(object sender, EventArgs e)
        {
            double[] a = dynamicWaveGraph.getSample();
            Series b = dynamicWaveGraph.getChartLabel();
            Calculations.createLowPassFilter(a.Length, (int)end);
            Calculations.convolve(a);
            a = dynamicWaveGraph.getSample();
            b.Points.Clear();
            dynamicWaveGraph.populateLineChart(a, b);
            dynamicWaveGraph d = new dynamicWaveGraph();
            d.Update();
        }

        public Series getFilterChart()
        {
            return filterChart;
        }

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

        private void filterSyncButton_Click(object sender, EventArgs e)
        {
            double[] a = dynamicWaveGraph.getSample();
            Series b = dynamicWaveGraph.getChartLabel();
            Calculations.createLowPassFilter(a.Length, (int)end);
            Calculations.convolveSync(a);
            a = dynamicWaveGraph.getSample();
            b.Points.Clear();
            dynamicWaveGraph.populateLineChart(a, b);
            dynamicWaveGraph d = new dynamicWaveGraph();
            d.Update();
        }

        private void iDFTButton_Click(object sender, EventArgs e)
        {
            double[] testArray = new double[30];
            Calculations.inverseDFT(testArray.Length, testArray);
        }

        private void iDFTSyncButton_Click(object sender, EventArgs e)
        {
            double[] testArray = new double[30];
            Calculations.inverseDFTSync(testArray.Length, testArray);
        }
    }
}
