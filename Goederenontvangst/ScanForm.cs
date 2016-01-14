using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Goederenontvangst
{
    public partial class ScanForm : Form
    {
        List<ScannedProduct> productList = new List<ScannedProduct>();

        public ScanForm()
        {
            InitializeComponent();

            laser1.GoodReadEvent += new datalogic.datacapture.ScannerEngine.LaserEventHandler(laser1_GoodReadEvent);
            laser1.ScannerEnabled = true;

            countTextBox.KeyPress += new KeyPressEventHandler(submitProductWithEnterKey);
            productTextBox.KeyPress += new KeyPressEventHandler(returnToMainMenu);

            productTextBox.Focus();
        }

        private void laser1_GoodReadEvent(datalogic.datacapture.ScannerEngine sender)
        {
            productTextBox.Text = laser1.BarcodeDataAsText;

            if (productTextBox.Text != String.Empty)
            {
                countTextBox.Enabled = true;
                countTextBox.Focus();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            DialogForm dialog = new DialogForm("test", "message");
            dialog.Location = new Point(0, 120);

            dialog.Show();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (productTextBox.Text != String.Empty && countTextBox.Text != String.Empty)
            {
                productList.Add(new ScannedProduct(productTextBox.Text, countTextBox.Text));

                countTextBox.Enabled = false;
                countTextBox.Focus();

                countTextBox.Text = productTextBox.Text = String.Empty;
            } 
        }

        private void createDialog(String title, String message)
        {
            DialogForm dialog = new DialogForm(title, message);
            dialog.Location = new Point(0, 80);

            dialog.Show();
        }

        private void saveProductToList()
        {
            if (productTextBox.Text != String.Empty && countTextBox.Text != String.Empty)
            {
                productList.Add(new ScannedProduct(productTextBox.Text, countTextBox.Text));

                countTextBox.Enabled = false;
                countTextBox.Focus();

                countTextBox.Text = productTextBox.Text = String.Empty;

                productTextBox.Focus();
            }
        }

        private void submitProductWithEnterKey(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                saveProductToList();
            }
        }

        private void returnToMainMenu(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.Close();
            }
        }
    }
}