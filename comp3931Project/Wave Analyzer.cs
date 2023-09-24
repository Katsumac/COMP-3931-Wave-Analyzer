namespace comp3931Project
{
    public partial class WaveAnalyzer : Form
    {
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
            filter.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 4, 520);



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

    }
}