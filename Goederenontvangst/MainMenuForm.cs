using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Goederenontvangst
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();

            this.KeyDown += new KeyEventHandler(MainMenuForm_KeyDown);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // Close the app
            this.Close();
        }

        private void MainMenuForm_KeyDown(object sender, KeyEventArgs key)
        {
            String pressedKey = key.KeyCode.ToString();

            if (pressedKey == "F1")
            {
                ScanForm scanform = new ScanForm();
                scanform.Show();
                key.Handled = true;
            } else if (pressedKey == "Escape")
            {
                this.Close();
                key.Handled = true;
            }
        }
    }
}