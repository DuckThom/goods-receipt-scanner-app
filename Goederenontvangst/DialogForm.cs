using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Goederenontvangst
{
    public partial class DialogForm : Form
    {
        public DialogForm(String title, String message)
        {
            InitializeComponent();

            titleTextBox.Text = title;
            messageTextBox.Text = message;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}