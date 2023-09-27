using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comp3931Project
{
    public partial class WaveWindow : Form
    {

        private Pen pen = new Pen(Color.Red, 2.0F);

        private List<PointF> wavePoints = new List<PointF> { };


        public WaveWindow()
        {
            InitializeComponent();
        }

        private void WaveWindow_Load(object sender, EventArgs e)
        {

        }

        private void ProcessSamples(double[] S)
        {

            for (int i = 0; i < S.Count(); i++)
            {
                float sineScale = WaveBox.Height / 2;
                wavePoints.Add(new PointF(i * 3, (float)S[i] * (sineScale / 3) + sineScale));
            }

        }

        private void ProcessSamples(int[] S)
        {

        }

        private void ProcessSamples(byte[] S)
        {

        }
        private void WaveBox_Paint(object sender, PaintEventArgs e)
        {
            ProcessSamples(Calculations.createSamples(2200, 100));
            PointF[] a = wavePoints.ToArray();
            e.Graphics.Clear(Color.White);
            if (wavePoints.Count > 0)

            {
                e.Graphics.DrawCurve(pen, a);
                e.Graphics.DrawLine(pen, 0, WaveBox.Height / 2, a[wavePoints.Count - 1].X, WaveBox.Height / 2);
            }
        }


    }
}
