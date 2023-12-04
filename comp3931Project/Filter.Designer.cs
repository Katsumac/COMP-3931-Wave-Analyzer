namespace comp3931Project
{
    partial class Filter
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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            contextMenuStrip1 = new ContextMenuStrip(components);
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            FilterButton = new Button();
            filterSyncButton = new Button();
            iDFTButton = new Button();
            iDFTSyncButton = new Button();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(2, 0);
            chart1.Margin = new Padding(0);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(1015, 252);
            chart1.TabIndex = 1;
            chart1.Text = "chart1";
            chart1.SelectionRangeChanged += chart1_SelectionRangeChanged;
            // 
            // FilterButton
            // 
            FilterButton.Location = new Point(887, 45);
            FilterButton.Name = "FilterButton";
            FilterButton.Size = new Size(94, 29);
            FilterButton.TabIndex = 2;
            FilterButton.Text = "Filter";
            FilterButton.UseVisualStyleBackColor = true;
            FilterButton.Click += FilterButton_Click;
            // 
            // filterSyncButton
            // 
            filterSyncButton.Location = new Point(887, 90);
            filterSyncButton.Name = "filterSyncButton";
            filterSyncButton.Size = new Size(94, 29);
            filterSyncButton.TabIndex = 3;
            filterSyncButton.Text = "Filter (Sync)";
            filterSyncButton.UseVisualStyleBackColor = true;
            filterSyncButton.Click += filterSyncButton_Click;
            // 
            // iDFTButton
            // 
            iDFTButton.Location = new Point(887, 135);
            iDFTButton.Name = "iDFTButton";
            iDFTButton.Size = new Size(94, 29);
            iDFTButton.TabIndex = 4;
            iDFTButton.Text = "iDFT";
            iDFTButton.UseVisualStyleBackColor = true;
            iDFTButton.Click += iDFTButton_Click;
            // 
            // iDFTSyncButton
            // 
            iDFTSyncButton.Location = new Point(887, 181);
            iDFTSyncButton.Name = "iDFTSyncButton";
            iDFTSyncButton.Size = new Size(94, 29);
            iDFTSyncButton.TabIndex = 5;
            iDFTSyncButton.Text = "iDFT (Sync)";
            iDFTSyncButton.UseVisualStyleBackColor = true;
            iDFTSyncButton.Click += iDFTSyncButton_Click;
            // 
            // Filter
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1017, 253);
            Controls.Add(iDFTSyncButton);
            Controls.Add(iDFTButton);
            Controls.Add(filterSyncButton);
            Controls.Add(FilterButton);
            Controls.Add(chart1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Filter";
            Text = "Filter";
            Load += Filter_Load;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Button FilterButton;
        private Button filterSyncButton;
        private Button iDFTButton;
        private Button iDFTSyncButton;
    }
}