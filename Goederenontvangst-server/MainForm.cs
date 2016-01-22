using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Goederenontvangst_server
{
    public partial class MainForm : Form
    {
        public delegate void UpdateTextCallback(string text);

        public MainForm()
        {
            InitializeComponent();

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.ipLabel.Text = "IP: " + addr.ToString();
                }
            }
        }

        private void setStatus(string text)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new UpdateTextCallback(this.setStatus), new object[] { text });
            }
            else
            {
                statusLabel.Text = text;
            }
        }
    }
}