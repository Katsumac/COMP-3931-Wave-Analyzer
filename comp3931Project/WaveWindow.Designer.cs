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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            WaveBoxPanel = new Panel();
            waveChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            WaveBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)waveChart).BeginInit();
            SuspendLayout();
            // 
            // WaveBoxPanel
            // 
            WaveBoxPanel.AutoScroll = true;
            WaveBoxPanel.Controls.Add(waveChart);
            WaveBoxPanel.Dock = DockStyle.Fill;
            WaveBoxPanel.Location = new Point(0, 0);
            WaveBoxPanel.Margin = new Padding(2, 1, 2, 1);
            WaveBoxPanel.Name = "WaveBoxPanel";
            WaveBoxPanel.Size = new Size(890, 265);
            WaveBoxPanel.TabIndex = 1;
            // 
            // waveChart
            // 
            chartArea1.Name = "ChartArea1";
            waveChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            waveChart.Legends.Add(legend1);
            waveChart.Location = new Point(0, 0);
            waveChart.Name = "waveChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            waveChart.Series.Add(series1);
            waveChart.Size = new Size(890, 265);
            waveChart.TabIndex = 0;
            waveChart.Text = "waveChart";
            // 
            // WaveWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(890, 265);
            Controls.Add(WaveBoxPanel);
            Margin = new Padding(2, 1, 2, 1);
            Name = "WaveWindow";
            Text = "WaveWindow";
            Load += WaveWindow_Load;
            WaveBoxPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)waveChart).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel WaveBoxPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart waveChart;
    }
}