namespace comp3931Project
{
    partial class WaveWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            WaveBoxPanel = new Panel();
            waveChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            WaveBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)waveChart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // WaveBoxPanel
            // 
            WaveBoxPanel.Controls.Add(chart1);
            WaveBoxPanel.Controls.Add(waveChart);
            WaveBoxPanel.Dock = DockStyle.Fill;
            WaveBoxPanel.Location = new Point(0, 0);
            WaveBoxPanel.Margin = new Padding(2, 1, 2, 1);
            WaveBoxPanel.Name = "WaveBoxPanel";
            WaveBoxPanel.Size = new Size(1075, 594);
            WaveBoxPanel.TabIndex = 1;
            // 
            // waveChart
            // 
            chartArea2.Name = "ChartArea1";
            waveChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            waveChart.Legends.Add(legend2);
            waveChart.Location = new Point(0, 0);
            waveChart.Name = "waveChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            waveChart.Series.Add(series2);
            waveChart.Size = new Size(888, 245);
            waveChart.TabIndex = 0;
            waveChart.Text = "waveChart";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(0, 251);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(888, 286);
            chart1.TabIndex = 1;
            chart1.Text = "chart1";
            // 
            // WaveWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1075, 594);
            Controls.Add(WaveBoxPanel);
            Margin = new Padding(2, 1, 2, 1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WaveWindow";
            ShowIcon = false;
            Text = "WaveWindow";
            WaveBoxPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)waveChart).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // TODO: maybe have a main window bool to see if it's the main window and if so Vdo below

            // check to see if there are any more windows in the array and set the locationt o here if there are
            base.OnFormClosing(e);
        }

        #endregion
        private Panel WaveBoxPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart waveChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}