using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BalieScanner
{
    public partial class DialogForm : Form
    {
        private bool hasInput;

        public DialogForm(String title, String message, bool hasInput)
        {
            InitializeComponent();

            this.hasInput = hasInput;

            titleTextBox.Text = title;
            messageTextBox.Text = message;

            if (hasInput)
            {
                textBox1.Visible = true;
                messageTextBox.Visible = false;
            }
            else
            {
                textBox1.Visible = false;
                messageTextBox.Visible = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (this.hasInput && textBox1.Text != String.Empty)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Goederenontvangst", "ServerIP", textBox1.Text);
            }

            this.Close();
        }
    }
}