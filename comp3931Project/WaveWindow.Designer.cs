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
            WaveBox = new PictureBox();
            WaveBoxPanel = new Panel();
            WaveWindowPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)WaveBox).BeginInit();
            WaveBoxPanel.SuspendLayout();
            WaveWindowPanel.SuspendLayout();
            SuspendLayout();
            // 
            // WaveBox
            // 
            WaveBox.BackColor = SystemColors.ControlDarkDark;
            WaveBox.Location = new Point(2, 1);
            WaveBox.Margin = new Padding(2, 1, 2, 1);
            WaveBox.Name = "WaveBox";
            WaveBox.Size = new Size(1073, 392);
            WaveBox.TabIndex = 0;
            WaveBox.TabStop = false;
            WaveBox.Paint += WaveBox_Paint;
            // 
            // WaveBoxPanel
            // 
            WaveBoxPanel.AutoScroll = true;
            WaveBoxPanel.Controls.Add(WaveWindowPanel);
            WaveBoxPanel.Dock = DockStyle.Fill;
            WaveBoxPanel.Location = new Point(0, 0);
            WaveBoxPanel.Margin = new Padding(2, 1, 2, 1);
            WaveBoxPanel.Name = "WaveBoxPanel";
            WaveBoxPanel.Size = new Size(890, 408);
            WaveBoxPanel.TabIndex = 1;
            // 
            // WaveWindowPanel
            // 
            WaveWindowPanel.AutoScroll = true;
            WaveWindowPanel.Controls.Add(WaveBox);
            WaveWindowPanel.Location = new Point(0, 3);
            WaveWindowPanel.Name = "WaveWindowPanel";
            WaveWindowPanel.Size = new Size(909, 401);
            WaveWindowPanel.TabIndex = 1;
            // 
            // WaveWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(890, 408);
            Controls.Add(WaveBoxPanel);
            Margin = new Padding(2, 1, 2, 1);
            Name = "WaveWindow";
            Text = "WaveWindow";
            Load += WaveWindow_Load;
            ((System.ComponentModel.ISupportInitialize)WaveBox).EndInit();
            WaveBoxPanel.ResumeLayout(false);
            WaveWindowPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WaveBox;
        private Panel WaveBoxPanel;
        private Panel WaveWindowPanel;
    }
}