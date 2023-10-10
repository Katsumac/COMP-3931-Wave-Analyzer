namespace comp3931Project
{
    partial class WaveAnalyzer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            surpriseToolStripMenuItem = new ToolStripMenuItem();
            saveToAudioFileToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            audioFileToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            filterToolStripMenuItem = new ToolStripMenuItem();
            waveDisplayToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            WaveAnalyzerPanel = new Panel();
            waveGraphToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, loadToolStripMenuItem, toolsToolStripMenuItem, settingsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(6, 3, 0, 3);
            menuStrip1.Size = new Size(1104, 30);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { surpriseToolStripMenuItem, saveToAudioFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // surpriseToolStripMenuItem
            // 
            surpriseToolStripMenuItem.Name = "surpriseToolStripMenuItem";
            surpriseToolStripMenuItem.Size = new Size(212, 26);
            surpriseToolStripMenuItem.Text = "Surprise!";
            surpriseToolStripMenuItem.Click += surpriseToolStripMenuItem_Click;
            // 
            // saveToAudioFileToolStripMenuItem
            // 
            saveToAudioFileToolStripMenuItem.Name = "saveToAudioFileToolStripMenuItem";
            saveToAudioFileToolStripMenuItem.Size = new Size(212, 26);
            saveToAudioFileToolStripMenuItem.Text = "Save to Audio File";
            saveToAudioFileToolStripMenuItem.Click += saveToAudioFileToolStripMenuItem_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { audioFileToolStripMenuItem, waveGraphToolStripMenuItem });
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(56, 24);
            loadToolStripMenuItem.Text = "Load";
            // 
            // audioFileToolStripMenuItem
            // 
            audioFileToolStripMenuItem.Name = "audioFileToolStripMenuItem";
            audioFileToolStripMenuItem.Size = new Size(159, 26);
            audioFileToolStripMenuItem.Text = "Audio File";
            audioFileToolStripMenuItem.Click += audioFileToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { filterToolStripMenuItem, waveDisplayToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(58, 24);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // filterToolStripMenuItem
            // 
            filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            filterToolStripMenuItem.Size = new Size(181, 26);
            filterToolStripMenuItem.Text = "Filter";
            filterToolStripMenuItem.Click += filterToolStripMenuItem_Click;
            // 
            // waveDisplayToolStripMenuItem
            // 
            waveDisplayToolStripMenuItem.Name = "waveDisplayToolStripMenuItem";
            waveDisplayToolStripMenuItem.Size = new Size(181, 26);
            waveDisplayToolStripMenuItem.Text = "Wave Display";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(76, 24);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // WaveAnalyzerPanel
            // 
            WaveAnalyzerPanel.AutoSize = true;
            WaveAnalyzerPanel.Location = new Point(0, 36);
            WaveAnalyzerPanel.Margin = new Padding(3, 4, 3, 4);
            WaveAnalyzerPanel.Name = "WaveAnalyzerPanel";
            WaveAnalyzerPanel.Size = new Size(1107, 481);
            WaveAnalyzerPanel.TabIndex = 3;
            WaveAnalyzerPanel.Paint += WaveAnalyzerPanel_Paint;
            // 
            // waveGraphToolStripMenuItem
            // 
            waveGraphToolStripMenuItem.Name = "waveGraphToolStripMenuItem";
            waveGraphToolStripMenuItem.Size = new Size(224, 26);
            waveGraphToolStripMenuItem.Text = "Wave Graph";
            waveGraphToolStripMenuItem.Click += waveGraphToolStripMenuItem_Click;
            // 
            // WaveAnalyzer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Navy;
            ClientSize = new Size(1104, 980);
            Controls.Add(WaveAnalyzerPanel);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "WaveAnalyzer";
            Text = "Wave Analyzer";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem surpriseToolStripMenuItem;

        private ToolStripMenuItem audioFileToolStripMenuItem;
        private ToolStripMenuItem saveToAudioFileToolStripMenuItem;
        private ToolStripMenuItem filterToolStripMenuItem;
        private ToolStripMenuItem waveDisplayToolStripMenuItem;

        private Panel WaveAnalyzerPanel;
        private ToolStripMenuItem waveGraphToolStripMenuItem;
    }
}