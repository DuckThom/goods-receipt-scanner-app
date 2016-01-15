using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Goederenontvangst
{
    public partial class MainMenuForm : Form
    {
        public List<ScannedProduct> productList = new List<ScannedProduct>();

        public MainMenuForm()
        {
            InitializeComponent();

            this.KeyDown += new KeyEventHandler(MainMenuForm_KeyDown);
        }

        private void MainMenuForm_KeyDown(object sender, KeyEventArgs key)
        {
            String pressedKey = key.KeyCode.ToString();

            if (pressedKey == "D1")
            {
                showScanForm();
            }
            else if (pressedKey == "D2")
            {
                
            }
            else if (pressedKey == "D3")
            {
                showSendForm();
            } 
            else if (pressedKey == "Escape")
            {
                closeApp();
            }
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            showScanForm();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            showSendForm();
        }  

        private void exitButton_Click(object sender, EventArgs e)
        {
            closeApp();
        }

        private void showScanForm()
        {
            ScanForm scanform = new ScanForm(this.productList);
            scanform.Show();
        }

        private void showSendForm()
        {
            if (productList.Count > 0)
            {
                SendForm sendform = new SendForm(this.productList, "192.168.1.29");
                sendform.Show();
            }
            else
            {
                MessageBeep();
            }
        }

        private void closeApp()
        {
            this.Close();
        }

        [DllImport("CoreDll.dll")]
        public static extern void MessageBeep(int code);

        public static void MessageBeep()
        {
            MessageBeep(-1);  // Default beep code is -1
        } 
    }
}