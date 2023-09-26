namespace comp3931Project
{
    public partial class WaveAnalyzer : Form
    {

        Pen pen = new Pen(Color.Red, 2.0F);

        List<PointF> wavePoints = new List<PointF> { };

        public WaveAnalyzer()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Filter filter = new Filter();
            filter.MdiParent = this;
            filter.TopLevel = false;
            filter.Show();
            filter.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 9, 490);
            filter.Size = new Size(1000, 330);

            foreach (Control control in this.Controls)
            {
                MdiClient client = control as MdiClient;
                if (client != null)
                {
                    client.BackColor = Color.Blue;
                    break;
                }
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private double ConvertToRadians(int angle)
        {
            return (Math.PI * angle) / 180;
        }

        private double GetSin(int angle)
        {
            double radians = ConvertToRadians(angle);
            return Math.Sin(radians);
        }

        private void CalculatePoints(int range)
        {
            for (int i = 0; i < range; i++)
            {
                float sineY = (float)GetSin(i) * (-1);

                float sineScale = WaveBox.Height / 2;
                wavePoints.Add(new PointF(i, sineY * sineScale + sineScale));
            }
        }

        private void WaveBox_Paint(object sender, PaintEventArgs e)
        {
            CalculatePoints(1260);
            e.Graphics.Clear(Color.White);
            if (wavePoints.Count > 0)
            {
                e.Graphics.DrawCurve(pen, wavePoints.ToArray());
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filter filter = new Filter();
            filter.MdiParent = this;
            filter.TopLevel = false;
            filter.Show();
            filter.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 9, 490);
            filter.Size = new Size(1000, 330);

        }
    }
}