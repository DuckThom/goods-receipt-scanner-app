namespace Goederenontvangst
{
    partial class DialogForm
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
            this.titleTextBox = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // titleTextBox
            // 
            this.titleTextBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.titleTextBox.Location = new System.Drawing.Point(3, 12);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(232, 20);
            this.titleTextBox.Text = "dialogTitle";
            this.titleTextBox.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(4, 36);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(231, 63);
            this.messageTextBox.Text = "dialogMessage";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(76, 102);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(90, 20);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Sluiten";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 56);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 23);
            this.textBox1.TabIndex = 5;
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(238, 135);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.titleTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogForm";
            this.Text = "Dialog";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label titleTextBox;
        private System.Windows.Forms.Label messageTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}