using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.MonthCalendar;

namespace comp3931Project
{
    public partial class WaveWindow : Form
    {

        private Pen pen = new Pen(Color.Red, 2.0F);

        private List<PointF> wavePoints = new List<PointF> { };

        private Wave wave;

        private static double[] filteredValues;
        private static Series frequency;
        private static double[] xValues;
        private static double[] yValues;
        private static double[] sample;
        private double start;
        private double end;


        public WaveWindow()
        {
            // create an id 
            InitializeComponent();
            waveChart.MouseWheel += waveChart_MouseWheel;



        }

        public void setWave(Wave w)
        {
            wave = w;
        }

        public Wave getWave()
        {
            return wave;
        }

        public void ChartWave(Wave w)
        {
            this.wave = w;
            double[] dataL = wave.getL();

            for (int i = 0; i < dataL.Length; i++)
            {
                waveChart.Series[0].Points.AddXY(i, dataL[i]);
            }



            double yMax = 0;
            double yMin = 0;

            //gets largest and smallest values for each of 
            foreach (double dp in dataL)
            {
                if (dp > yMax)
                {
                    yMax = dp;
                }
                else if (dp < yMin)
                {
                    yMin = dp;
                }
            }


            double yAxisMax = 0;
            double yAxisMin = 0;
            double yAxisMaxCur = 0.01;
            double yAxisMinCur = 0;
            if (yMax > yAxisMaxCur && yMax > 0)
            {
                if (yMax > 1)
                {
                    yAxisMax = Math.Truncate(yMax + yMax / 2);
                    while (yAxisMax < yMax)
                    {
                        yAxisMax++;
                    }
                }
                else
                {
                    yAxisMaxCur = Math.Truncate(yMax * 100) / 100;
                    yAxisMax = yAxisMaxCur + yAxisMaxCur / 2;
                    if (yAxisMax > 1 && yAxisMax > yMax)
                    {
                        yAxisMax = Math.Truncate(yAxisMax);
                    }
                }
            }
            if (yAxisMax == 0)
            {
                yAxisMax = 0.01;
            }
            if (yMin < yAxisMinCur && yMin < 0)
            {
                if (yMin < -1)
                {
                    yAxisMin = Math.Truncate(yMin + yMin / 2);
                }
                else
                {
                    yAxisMinCur = Math.Truncate(yMin * 100) / 100;
                    yAxisMin = yAxisMinCur + yAxisMinCur / 2;
                    if (yAxisMin > 1 && yAxisMin > yMin)
                    {
                        yAxisMin = Math.Truncate(yAxisMin);
                    }
                }
            }

            if (yAxisMax > yAxisMin * -1 && yAxisMin != 0)
            {
                yAxisMin = -1 * yAxisMax;
            }
            else if (yAxisMin == 0)
            {
            }
            else
            {
                yAxisMax = -1 * yAxisMin;
            }

            waveChart.ChartAreas[0].AxisX.Minimum = 0;
            //  waveChart.ChartAreas[0].AxisX.Maximum = dataL.Length;
            // Configure the X-axis for scrolling and zooming
            waveChart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            waveChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            waveChart.ChartAreas[0].AxisY.ScrollBar.Enabled = true;
            waveChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            waveChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            waveChart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = dataL.Length - 2;
            waveChart.ChartAreas[0].CursorX.AutoScroll = true;
            // waveChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            // Set initial visible range (adjust as needed)
            waveChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, dataL.Length / 2);
            waveChart.ChartAreas[0].AxisY.ScaleView.Zoom(yAxisMin, yAxisMax);
            int windowHeight = this.Height;
            /* waveChart.ChartAreas[0].AxisY.Maximum = yAxisMax;
             waveChart.ChartAreas[0].AxisY.Minimum = yAxisMin;
 */
            double interval = yAxisMax;
            waveChart.Series[0].ChartType = SeriesChartType.FastLine;

            /*waveChart.Series[0].ChartType = SeriesChartType.FastLine;
            //   waveChart.ChartAreas[0].CursorX.AutoScroll = true;
            waveChart.ChartAreas[0].AxisY.MajorGrid.Interval = interval;
            waveChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 2;
            waveChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 2;
            waveChart.ChartAreas[0].AxisY.Interval = interval;
            //waveChart.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
           // waveChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            //   waveChart.ChartAreas[0].AxisX.ScaleView.Zoom(1, dataL.Length - 1);

            int pos = (int)waveChart.ChartAreas[0].AxisX.ScaleView.Position;

            waveChart.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            waveChart.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            waveChart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = dataL.Length - 2;
            waveChart.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = dataL.Length - 2;
*/


        }

        // https://stackoverflow.com/questions/13584061/how-to-enable-zooming-in-microsoft-chart-control-by-using-mouse-wheel
        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XFinish { get; set; }
            public double YStart { get; set; }
            public double YFinish { get; set; }
        }

        private readonly Stack<ZoomFrame> _zoomFrames = new Stack<ZoomFrame>();

        private void waveChart_MouseWheel(object sender, MouseEventArgs e)
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



        private void ProcessSamples(double[] S)
        {

            /* 
             private void dynamicWaveGraph_Load(object sender, EventArgs e)
        {
            sample = Calculations.createSamples(30, 8);

            const int pageSize = 10;

            // clear the chart
            chart1.Series.Clear();

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

        public static void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]/sample.Max());
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
             
             
             
             */

        }

        private void ProcessSamples(int[] S)
        {

        }

        private void ProcessSamples(byte[] S)
        {

        }


    }
}
