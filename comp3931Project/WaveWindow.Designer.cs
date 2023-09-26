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
            ((System.ComponentModel.ISupportInitialize)WaveBox).BeginInit();
            WaveBoxPanel.SuspendLayout();
            SuspendLayout();
            // 
            // WaveBox
            // 
            WaveBox.BackColor = SystemColors.ControlDarkDark;
            WaveBox.Location = new Point(3, 3);
            WaveBox.Name = "WaveBox";
            WaveBox.Size = new Size(1643, 327);
            WaveBox.TabIndex = 0;
            WaveBox.TabStop = false;
            WaveBox.Paint += WaveBox_Paint;
            // 
            // WaveBoxPanel
            // 
            WaveBoxPanel.AutoScroll = true;
            WaveBoxPanel.Controls.Add(WaveBox);
            WaveBoxPanel.Dock = DockStyle.Fill;
            WaveBoxPanel.Location = new Point(0, 0);
            WaveBoxPanel.Name = "WaveBoxPanel";
            WaveBoxPanel.Size = new Size(1883, 939);
            WaveBoxPanel.TabIndex = 1;
            // 
            // WaveWindow
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1883, 939);
            Controls.Add(WaveBoxPanel);
            Name = "WaveWindow";
            Text = "WaveWindow";
            Load += WaveWindow_Load;
            ((System.ComponentModel.ISupportInitialize)WaveBox).EndInit();
            WaveBoxPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WaveBox;
        private Panel WaveBoxPanel;
    }
}