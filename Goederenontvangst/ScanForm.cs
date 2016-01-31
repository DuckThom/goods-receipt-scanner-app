using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using datalogic.datacapture;

namespace Goederenontvangst
{
    public partial class ScanForm : Form
    {
        List<ScannedProduct> productList;
        String scannerInput;
        Laser laser;
        bool replace = false;

        public ScanForm(List<ScannedProduct> productList, Laser laser)
        {
            InitializeComponent();

            this.productList = productList;
            this.laser = laser;

            this.laser.GoodReadEvent += new ScannerEngine.LaserEventHandler(laser_GoodReadEvent);
            this.laser.ScannerEnabled = true;

            this.KeyDown += new KeyEventHandler(ScanMenuForm_KeyDown);
            countTextBox.KeyPress += new KeyPressEventHandler(submitProductWithEnterKey);

            productTextBox.Focus();
        }

        private void ScanMenuForm_KeyDown(object sender, KeyEventArgs key)
        {
            String pressedKey = key.KeyCode.ToString();

            if (pressedKey == "Escape")
            {
                this.laser.ScannerEnabled = false;
                this.laser.GoodReadEvent -= laser_GoodReadEvent;
                this.Close();
                key.Handled = true;
            }
        }

        private void laser_GoodReadEvent(ScannerEngine sender)
        {
            this.scannerInput = laser.BarcodeDataAsText;

            productTextBox.Text = scannerInput;

            if (this.productList.Exists(findScannedProduct))
            {
                countTextBox.Text = this.productList.Find(findScannedProduct).getCount();
                this.replace = true;
            }

            if (productTextBox.Text != String.Empty)
            {
                countTextBox.Enabled = true;
                countTextBox.Focus();
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            saveProductToList();
        }

        private void saveProductToList()
        {
            if (productTextBox.Text != String.Empty && countTextBox.Text != String.Empty)
            {
                if (this.replace)
                {
                    this.productList.Find(findScannedProduct).setCount(countTextBox.Text);
                    this.replace = false;
                }
                //else if (Convert.ToDouble(countTextBox.Text) <= 0.0)
                //{
                //   this.productList.Remove(this.productList.Find(findScannedProduct));
                //}
                else
                {
                    ScannedProduct sp = new ScannedProduct(productTextBox.Text);
                    sp.setCount(countTextBox.Text);

                    this.productList.Add(sp);
                }  

                countTextBox.Enabled = false;

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

        private bool findScannedProduct(ScannedProduct obj)
        {
            return obj.getProduct() == this.scannerInput;
        }
    }
}