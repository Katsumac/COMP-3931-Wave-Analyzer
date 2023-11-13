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
using ScottPlot;

namespace comp3931Project
{
    public partial class dynamicWaveGraph2 : Form
    {

        private Series frequency;
        private dynamicWaveGraph dynamicWaveGraph = new dynamicWaveGraph();
        private static double[] sample;

        public dynamicWaveGraph2()
        {
            InitializeComponent();
        }

        private void dynamicWaveGraph2_Load(object sender, EventArgs e)
        {
            sample = Calculations.createSamples(30, 8);

            /*            formsPlot1.Plot.AddSignal(sample, 20_000);
                        formsPlot1.Refresh();*/

            const int pageSize = 10;

            // clear the chart
            chart1.Series.Clear();

            // Populate the bar chart chart
            frequency = chart1.Series.Add("Frequency");
            frequency.ChartType = SeriesChartType.Spline;

            frequency.Points.AddXY(1, 1);

            /*populateLineChart(sample, frequency);*/

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[frequency.ChartArea];

            customizeLineChart(pageSize, filterChartArea, sample);

            chart1.MouseWheel += chart1_MouseWheel;
        }

        private void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]);
        }

        private void pasteLineChart(double[] xValues, double[] yValues, Series chartLabel)
        {
            for (int i = 0; i < xValues.Length; i++)
                chartLabel.Points.AddXY(xValues[i], yValues[i]);
        }

        private void customizeLineChart(int pageSize, ChartArea chartArea, double[] sample)
        {
            // How much data we want
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = sample.Length;

            // Enables scrolling
            chartArea.CursorX.AutoScroll = true;

            // How much we see on one page
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.AxisX.ScaleView.Zoom(0, pageSize);
            chartArea.AxisX.Interval = 1;

            // Works with Zoomable to allow zooming via highlighting
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;

            // Sets the thumb style
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // Small scrolling size
            chartArea.AxisX.ScaleView.SmallScrollSize = pageSize;
        }

        /*      For line chart scrolling*/
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
                xAxis.ScaleView.Zoom(0, 10);
                yAxis.ScaleView.ZoomReset();
            }
            else if (e.Delta > 0)
            {
                xAxis.ScaleView.Zoom(Math.Floor(posXStart), Math.Floor(posXFinish));
                yAxis.ScaleView.Zoom(0, yMax - 1);
            }
        }

        private void DFTButton_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.getFilterChart().Points.Clear();
            filter.populateBarChart(Calculations.DFT(dynamicWaveGraph.getYValues(), dynamicWaveGraph.getYValues().Length), filter.getFilterChart());
            filter.getFilterChart().Color = Color.CornflowerBlue;
            filter.Filter_Load(sender, e);
        }

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                frequency.Points.Clear();
                Debug.WriteLine("Yay!");
                pasteLineChart(dynamicWaveGraph.getXValues(), dynamicWaveGraph.getYValues(), frequency);
            }
        }
    }
}
