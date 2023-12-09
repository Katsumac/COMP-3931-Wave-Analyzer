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
            FileOpenToolStripMenuItem = new ToolStripMenuItem();
            saveToAudioFileToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            ToolRecordButton = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(1104, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { FileOpenToolStripMenuItem, saveToAudioFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // FileOpenToolStripMenuItem
            // 
            FileOpenToolStripMenuItem.Name = "FileOpenToolStripMenuItem";
            FileOpenToolStripMenuItem.Size = new Size(168, 22);
            FileOpenToolStripMenuItem.Text = "Open";
            FileOpenToolStripMenuItem.Click += FileOpenToolStripMenuItem_Click;
            // 
            // saveToAudioFileToolStripMenuItem
            // 
            saveToAudioFileToolStripMenuItem.Name = "saveToAudioFileToolStripMenuItem";
            saveToAudioFileToolStripMenuItem.Size = new Size(168, 22);
            saveToAudioFileToolStripMenuItem.Text = "Save to Audio File";
            saveToAudioFileToolStripMenuItem.Click += saveToAudioFileToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ToolRecordButton });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            toolsToolStripMenuItem.Click += toolsToolStripMenuItem_Click;
            // 
            // ToolRecordButton
            // 
            ToolRecordButton.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem });
            ToolRecordButton.Name = "ToolRecordButton";
            ToolRecordButton.Size = new Size(180, 22);
            ToolRecordButton.Text = "Record";
            ToolRecordButton.MouseEnter += ToolRecordButton_MouseEnter;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(98, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // WaveAnalyzer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Navy;
            ClientSize = new Size(1104, 791);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "WaveAnalyzer";
            Text = "Wave Analyzer";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem FileOpenToolStripMenuItem;
        private ToolStripMenuItem saveToAudioFileToolStripMenuItem;
        private ToolStripMenuItem ToolRecordButton;
        private ToolStripMenuItem newToolStripMenuItem;
    }
}