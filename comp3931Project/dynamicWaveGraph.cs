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

        private static double[] filteredValues;
        private static Series frequency;
        private static double[] xValues;
        private static double[] yValues;
        private static double[] sample;
        private double start;
        private double end;

        public dynamicWaveGraph()
        {
            InitializeComponent();
        }

        private void dynamicWaveGraph_Load(object sender, EventArgs e)
        {
            sample = Calculations.createSamples(30, 8);

            const int pageSize = 10;

            // clear the chart
            chart1.Series.Clear();

            // Populate the bar chart chart
            frequency = chart1.Series.Add("Frequency");
            frequency.ChartType = SeriesChartType.Spline;

            populateLineChart(sample, 0, frequency);

            // Customize the bar chart
            ChartArea filterChartArea = chart1.ChartAreas[frequency.ChartArea];
            customizeLineChart(pageSize, filterChartArea, sample);

            chart1.MouseWheel += chart1_MouseWheel;
            chart1.SelectionRangeChanged += Chart_SelectionRangeChanged;
        }

        public static void populateLineChart(double[] sample, int startIndex, Series chartLabel)
        {
            for (int i = startIndex; i < sample.Length; i++)
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

        public double[] getXValues()
        {
            return xValues;
        }
        public double[] getYValues()
        {
            return yValues;
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

        private void DFTButton_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.getFilterChart().Points.Clear();
            double[] DFTSamples = Calculations.DFT(sample, sample.Length);
            filter.populateBarChart(DFTSamples, filter.getFilterChart());
            filter.getFilterChart().Color = Color.Green;
            filter.Filter_Load(sender, e);
        }

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.C && e.Control)
            {

                Debug.WriteLine("This is the start: " + start);
                Debug.WriteLine("This is the end: " + end);

                /*                double[] A = Calculations.createSamples(30, 8);
                */
                int range = (int)(end - start) + 1;

                /*            filteredValues = new double[range + 1];*/

                xValues = new double[range];
                yValues = new double[range];

                for (int i = 0; i < range; i++)
                {
                    xValues[i] = frequency.Points[(int)(start + i)].GetValueByName("X");
                    yValues[i] = frequency.Points[(int)(start + i)].GetValueByName("Y");

                    Debug.WriteLine("(" + xValues[i] + ", " + yValues[i] + ")");
                }


                copySelection(yValues);

            }
            else if (e.KeyCode == Keys.X && e.Control)
            {

                Debug.WriteLine("This is the start: " + start);
                Debug.WriteLine("This is the end: " + end);

                /*                double[] A = Calculations.createSamples(30, 8);
                */
                int range = (int)(end - start) + 1;

                /*            filteredValues = new double[range + 1];*/

                xValues = new double[range];
                yValues = new double[range];

                for (int i = 0; i < range; i++)
                {
                    xValues[i] = frequency.Points[(int)(start + i)].GetValueByName("X");
                    yValues[i] = frequency.Points[(int)(start + i)].GetValueByName("Y");
                    sample[i] = 0;
                    Debug.WriteLine("(" + xValues[i] + ", " + yValues[i] + ")");
                }



                copySelection(yValues);

            }
            else if (e.KeyCode == Keys.V && e.Control)
            {

                    Debug.WriteLine("Yay!");
                    pasteLineChart(xValues, retrieveData(), (int)start, frequency);
                

            }
        }

        private void pasteLineChart(double[] xValues, double[] yValues, int startIndex, Series chartLabel)
        {
            int yIndex = 0;
            for (int i = startIndex; i < startIndex + xValues.Length - 1; i++)
            {
                sample[i] = yValues[yIndex];
                yIndex++;
                
/*                chartLabel.Points.AddXY(startIndex, yValues[i]);
                startIndex++;*/
            }
            chartLabel.Points.Clear();
            populateLineChart(sample, 0, chartLabel);

        }

        private void copySelection(double[] yValues)
        {
            string data = string.Join(",", yValues);
            Clipboard.SetText(data);
        }

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

        public static double[] getSample()
        {
            return sample;
        }

        public static void setSample(double[] newSample)
        {
            sample = newSample;
        }

        public static Series getChartLabel()
        {
            return frequency;
        }
        
    }
}
