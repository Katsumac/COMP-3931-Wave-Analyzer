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

namespace comp3931Project
{
    public partial class dynamicWaveGraph : Form
    {
        public dynamicWaveGraph()
        {
            InitializeComponent();
        }

        private void dynamicWaveGraph_Load(object sender, EventArgs e)
        {
            double[] sample = Calculations.createSamples(30, 8);

            const int pageSize = 10;

            // clear the chart
            chart1.Series.Clear();

            // Populate the bar chart chart
            Series filterChart = chart1.Series.Add("Frequency");
            filterChart.ChartType = SeriesChartType.Spline;
          
            populateLineChart(sample, filterChart);

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[filterChart.ChartArea];
            customizeLineChart(pageSize, filterChartArea, sample);

            chart1.MouseWheel += chart1_MouseWheel;
        }

        private void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]);
        }

        private void customizeLineChart(int pageSize, ChartArea chartArea, double[] sample)
        {
            // How much data we want
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = sample.Length;

            // Enables scrolling
            chartArea.CursorX.AutoScroll = true;

            // How much we see on one page
            chartArea.AxisX.ScaleView.Zoomable = true;
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
    }
}
