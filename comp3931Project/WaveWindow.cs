using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
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

        private Wave wave;

        private static Series amplitude;
        private static double[] xValues;
        private static double[] yValues;
        private static double[] dataL;
        private double start;
        private double end;
        private bool isRectangleWindowing = true; // Toggles between rectangular and triangular windowing. Default is rectangular windowing


        public WaveWindow()
        {
            // create an id 
            InitializeComponent();
            waveChart.MouseWheel += waveChart_MouseWheel;
            waveChart.SelectionRangeChanged += waveChart_SelectionRangeChanged;
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
            dataL = wave.getL();

            waveChart.Series.Clear();

            amplitude = waveChart.Series.Add("Amplitude");

            ChartArea waveChartChartArea = waveChart.ChartAreas[amplitude.ChartArea];
            waveChartChartArea.AxisX.Title = "Sample #";
            waveChartChartArea.AxisY.Title = "Amplitude";

            for (int i = 0; i < dataL.Length; i++)
            {
                amplitude.Points.AddXY(i, dataL[i]);
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
            waveChart.ChartAreas[0].AxisX.ScaleView.Zoomable = false;
            waveChart.ChartAreas[0].AxisY.ScrollBar.Enabled = true;
            waveChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            waveChart.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            waveChart.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = dataL.Length - 2;
            waveChart.ChartAreas[0].CursorX.AutoScroll = true;
            // waveChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;


            waveChart.ChartAreas[0].CursorX.IsUserEnabled = true;
            waveChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            waveChart.ChartAreas[0].CursorX.AutoScroll = true; // Enables scrolling

            // Set initial visible range (adjust as needed)
            waveChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, dataL.Length / 2);
            waveChart.ChartAreas[0].AxisY.ScaleView.Zoom(yAxisMin, yAxisMax);
            int windowHeight = this.Height;
            /* waveChart.ChartAreas[0].AxisY.Maximum = yAxisMax;
             waveChart.ChartAreas[0].AxisY.Minimum = yAxisMin;
 */
            double interval = yAxisMax;
            amplitude.ChartType = SeriesChartType.FastLine;

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

        public static void populateLineChart(double[] sample, Series chartLabel)
        {
            for (int i = 0; i < sample.Length; i++)
                chartLabel.Points.AddXY(i, sample[i]);
        }

        /**
         * Purpose: Updates the range selected by the user
         * 
         * @param sender: The object that raised the event
         * @param e: Contains cursor event data
         * 
         * @return: None
         */
        private void waveChart_SelectionRangeChanged(object sender, CursorEventArgs e)
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
            if (isRectangleWindowing)
            {
                applyRectangularWindow();
            }
            else
            {
                applyTriangularWindow();
            }
            double[] DFTSamples = Calculations.DFT(dataL);
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
            if (isRectangleWindowing)
            {
                applyRectangularWindow();
            }
            else
            {
                applyTriangularWindow();
            }
            double[] DFTSamples = Calculations.DFTSync(dataL);
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
        private void waveChart_KeyDown(object sender, KeyEventArgs e)
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
                pasteLineChart(xValues, retrieveData(), amplitude);
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
                xValues[i] = amplitude.Points[(int)(start + i)].GetValueByName("X");
                yValues[i] = amplitude.Points[(int)(start + i)].GetValueByName("Y");

                if (isCut)
                {
                    dataL[i] = 0;
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
            for (int i = (int)start; i < xValues.Length + start; i++)
            {
                dataL[i] = yValues[yIndex];
                yIndex++;
            }
            chartLabel.Points.Clear();
            populateLineChart(dataL, chartLabel);
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
            return dataL;
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
            dataL = newSample;
        }

        /**
         * Purpose: Returns the chart label
         * 
         * @return: The chart label
         */
        public static Series getChartLabel()
        {
            return amplitude;
        }

        /**
         * Purpose: Performs the triangle windowing on the samples
         * 
         * @return: None
         */
        private void applyTriangularWindow()
        {
            int N = dataL.Length;
            for (int n = 0; n < N; n++)
            {
                dataL[n] *= 1.0 - Math.Abs((n - N / 2) / (N / 2));
            }
        }

        /**
        * Purpose: Performs rectangle windowing on the samples
        * 
        * @return: None
        */
        private void applyRectangularWindow()
        {
            int N = dataL.Length;
            for (int n = 0; n < N; n++)
            {
                dataL[n] *= 1;
            }
        }

        /**
         * Purpose: Changes the boolean isRectangleWindow to toggle to rectangular windowing
         * 
         * @param sender: The object that raised the event
         * @param e: Contains key event data
         * 
         * @return: None
         */
        private void RectangleWindowButton_CheckedChanged(object sender, EventArgs e)
        {
            isRectangleWindowing = true;
        }

        /**
         * Purpose: Changes the boolean isRectangleWindow to toggle to triangular windowing
         * 
         * @param sender: The object that raised the event
         * @param e: Contains key event data
         * 
         * @return: None
         */
        private void TriangleWindowButton_CheckedChanged(object sender, EventArgs e)
        {
            isRectangleWindowing = false;
        }

        

    }
}
