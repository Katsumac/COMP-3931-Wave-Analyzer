namespace comp3931Project
{
    partial class dynamicWaveGraph
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
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            DFTButton = new Button();
            DFTSyncButton = new Button();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(0, 0);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(1016, 253);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            chart1.KeyDown += chart1_KeyDown;
            // 
            // DFTButton
            // 
            DFTButton.Location = new Point(887, 52);
            DFTButton.Name = "DFTButton";
            DFTButton.Size = new Size(94, 29);
            DFTButton.TabIndex = 1;
            DFTButton.Text = "DFT";
            DFTButton.UseVisualStyleBackColor = true;
            DFTButton.Click += DFTButton_Click;
            // 
            // DFTSyncButton
            // 
            DFTSyncButton.Location = new Point(887, 106);
            DFTSyncButton.Name = "DFTSyncButton";
            DFTSyncButton.Size = new Size(94, 29);
            DFTSyncButton.TabIndex = 2;
            DFTSyncButton.Text = "DFT (Sync)";
            DFTSyncButton.UseVisualStyleBackColor = true;
            DFTSyncButton.Click += DFTSyncButton_Click;
            // 
            // dynamicWaveGraph
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1017, 253);
            Controls.Add(DFTSyncButton);
            Controls.Add(DFTButton);
            Controls.Add(chart1);
            Name = "dynamicWaveGraph";
            Text = "dynamicWaveGraph";
            Load += dynamicWaveGraph_Load;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Button DFTButton;
        private Button DFTSyncButton;
    }
}