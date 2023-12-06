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
            TriangleWindowButton = new RadioButton();
            RectangleWindowButton = new RadioButton();
            DFTSyncButton = new Button();
            DFTButton = new Button();
            waveChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            WaveBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)waveChart).BeginInit();
            SuspendLayout();
            // 
            // WaveBoxPanel
            // 
            WaveBoxPanel.BackColor = Color.Transparent;
            WaveBoxPanel.Controls.Add(TriangleWindowButton);
            WaveBoxPanel.Controls.Add(RectangleWindowButton);
            WaveBoxPanel.Controls.Add(DFTSyncButton);
            WaveBoxPanel.Controls.Add(DFTButton);
            WaveBoxPanel.Controls.Add(waveChart);
            WaveBoxPanel.Dock = DockStyle.Fill;
            WaveBoxPanel.Location = new Point(0, 0);
            WaveBoxPanel.Margin = new Padding(2, 1, 2, 1);
            WaveBoxPanel.Name = "WaveBoxPanel";
            WaveBoxPanel.Size = new Size(1015, 325);
            WaveBoxPanel.TabIndex = 1;
            // 
            // TriangleWindowButton
            // 
            TriangleWindowButton.AutoSize = true;
            TriangleWindowButton.BackColor = SystemColors.Window;
            TriangleWindowButton.Location = new Point(839, 219);
            TriangleWindowButton.Name = "TriangleWindowButton";
            TriangleWindowButton.Size = new Size(163, 24);
            TriangleWindowButton.TabIndex = 4;
            TriangleWindowButton.TabStop = true;
            TriangleWindowButton.Text = "Triangle Windowing";
            TriangleWindowButton.UseVisualStyleBackColor = false;
            TriangleWindowButton.CheckedChanged += TriangleWindowButton_CheckedChanged;
            // 
            // RectangleWindowButton
            // 
            RectangleWindowButton.AutoSize = true;
            RectangleWindowButton.BackColor = SystemColors.Window;
            RectangleWindowButton.Location = new Point(839, 166);
            RectangleWindowButton.Name = "RectangleWindowButton";
            RectangleWindowButton.Size = new Size(176, 24);
            RectangleWindowButton.TabIndex = 3;
            RectangleWindowButton.TabStop = true;
            RectangleWindowButton.Text = "Rectangle Windowing";
            RectangleWindowButton.UseVisualStyleBackColor = false;
            RectangleWindowButton.CheckedChanged += RectangleWindowButton_CheckedChanged;
            // 
            // DFTSyncButton
            // 
            DFTSyncButton.Location = new Point(883, 113);
            DFTSyncButton.Name = "DFTSyncButton";
            DFTSyncButton.Size = new Size(94, 29);
            DFTSyncButton.TabIndex = 2;
            DFTSyncButton.Text = "DFT (Sync)";
            DFTSyncButton.UseVisualStyleBackColor = true;
            DFTSyncButton.Click += DFTSyncButton_Click;
            // 
            // DFTButton
            // 
            DFTButton.Location = new Point(883, 61);
            DFTButton.Name = "DFTButton";
            DFTButton.Size = new Size(94, 29);
            DFTButton.TabIndex = 1;
            DFTButton.Text = "DFT";
            DFTButton.UseVisualStyleBackColor = true;
            DFTButton.Click += DFTButton_Click;
            // 
            // waveChart
            // 
            chartArea1.Name = "ChartArea1";
            waveChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            waveChart.Legends.Add(legend1);
            waveChart.Location = new Point(0, 0);
            waveChart.Margin = new Padding(0);
            waveChart.Name = "waveChart";
            waveChart.Padding = new Padding(5);
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            waveChart.Series.Add(series1);
            waveChart.Size = new Size(1015, 327);
            waveChart.TabIndex = 0;
            waveChart.Text = "waveChart";
            waveChart.SelectionRangeChanged += waveChart_SelectionRangeChanged;
            waveChart.KeyDown += waveChart_KeyDown;
            // 
            // WaveWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1015, 325);
            Controls.Add(WaveBoxPanel);
            Margin = new Padding(2, 1, 2, 1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WaveWindow";
            ShowIcon = false;
            Text = "WaveWindow";
            WaveBoxPanel.ResumeLayout(false);
            WaveBoxPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)waveChart).EndInit();
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
        private Button DFTButton;
        private Button DFTSyncButton;
        private RadioButton RectangleWindowButton;
        private RadioButton TriangleWindowButton;
    }
}