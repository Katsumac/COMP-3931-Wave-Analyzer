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
    public partial class Filter : Form
    {
        public Filter()
        {
            InitializeComponent();
        }

        private void Filter_Load(object sender, EventArgs e)
        {
            Series frequency = chart1.Series.Add("Frequency");
            double[] A = Calculations.DFT(Calculations.createSamples(30, 8), 30);
            fillBarChart(A, frequency);
            chart1.Size = new Size(1000, 280);
            Legend legend = new Legend();
            legend.Enabled = false;
        }

        private void fillBarChart(double[] A, Series frequency)
        {
            for (int f = 0; f < A.Length; f++)
            {
                frequency.Points.AddXY(f, A[f]);
            }
        }
    }
}
