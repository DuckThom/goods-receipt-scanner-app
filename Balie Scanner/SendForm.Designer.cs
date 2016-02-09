namespace BalieScanner
{
    partial class SendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendForm));
            this.headerLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.radioSignal1 = new datalogic.wireless.RadioSignal();
            this.terminateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // headerLabel
            // 
            this.headerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(8)))), ((int)(((byte)(48)))));
            this.headerLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.Location = new System.Drawing.Point(3, 21);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(234, 20);
            this.headerLabel.Text = "Status";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(8)))), ((int)(((byte)(48)))));
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(3, 52);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(234, 105);
            this.statusLabel.Text = "Status";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // radioSignal1
            // 
            this.radioSignal1.Active = true;
            this.radioSignal1.DrawDir = datalogic.wireless.EDrawDir.ddHorizontal;
            this.radioSignal1.FillColor = System.Drawing.Color.Green;
            this.radioSignal1.Location = new System.Drawing.Point(3, 321);
            this.radioSignal1.LowLevelColor = System.Drawing.Color.Red;
            this.radioSignal1.Name = "radioSignal1";
            this.radioSignal1.Size = new System.Drawing.Size(234, 28);
            this.radioSignal1.TabIndex = 3;
            this.radioSignal1.Text = "radioSignal1";
            this.radioSignal1.TextInside = true;
            this.radioSignal1.Threshold = 20;
            // 
            // terminateButton
            // 
            this.terminateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(8)))), ((int)(((byte)(48)))));
            this.terminateButton.ForeColor = System.Drawing.Color.White;
            this.terminateButton.Location = new System.Drawing.Point(0, 0);
            this.terminateButton.Name = "terminateButton";
            this.terminateButton.Size = new System.Drawing.Size(39, 41);
            this.terminateButton.TabIndex = 7;
            this.terminateButton.Text = "X";
            this.terminateButton.Click += new System.EventHandler(this.terminateButton_Click);
            // 
            // SendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(8)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.ControlBox = false;
            this.Controls.Add(this.terminateButton);
            this.Controls.Add(this.radioSignal1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendForm";
            this.Text = "Balie Scanner : Verzenden";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label statusLabel;
        private datalogic.wireless.RadioSignal radioSignal1;
        private System.Windows.Forms.Button terminateButton;
    }
}